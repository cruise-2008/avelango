using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;
using TaskStatus = Avelango.Models.Enums.TaskStatus;

namespace Avelango.DbOrm.Implementation
{
    public class ImpTasks : ITasks
    {
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<TaskBids> _taskBids;
        private readonly IRepository<TaskAttachments> _tasksAttachment;
        private readonly IRepository<Users> _users;

        public ImpTasks(IRepository<Tasks> tasks, IRepository<Users> users, IRepository<TaskAttachments> tasksAttachment, IRepository<TaskBids> taskBids) {
            _tasks = tasks;
            _users = users;
            _tasksAttachment = tasksAttachment;
            _taskBids = taskBids;
        }


        public OperationResult<ApplicationTask> GetTaskByPk(Guid userPk, Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("[ImpTasks]/[GetTaskByPk] Task with PublicKey: " + taskPk + " does not found."));
                var atask = (ApplicationTask) task;
                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                atask.Customer = (ApplicationCustomer)customer;
                atask.Attachments = _tasksAttachment.GetFiltered(x => x.BelongsToTask == task.ID).Select(att => (ApplicationTaskAttachment)att).ToList();
                if (userPk == Guid.Empty) return new OperationResult<ApplicationTask>(atask); {
                    var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                    if (user != null) {
                        atask.AlreadyBided = _taskBids.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID) != 0;
                    }
                }
                return new OperationResult<ApplicationTask>(atask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }


        public OperationResult<IEnumerable<ApplicationTask>> GetFilteredTasks(Guid userPk, List<Guid> viewedTasks, List<string> subgroups, bool justActual, int n, string country, string city) {
            try {
                var states = justActual
                    ? new List<string> {TaskStatus.Open.ToString()}
                    : new List<string> {TaskStatus.Open.ToString(), TaskStatus.Expired.ToString(), TaskStatus.Closed.ToString(), TaskStatus.InProgress.ToString() };
                var result = new List<ApplicationTask>();

                IEnumerable<Tasks> tasks;
                if (viewedTasks.Any()) {
                    if (subgroups.Any()) {
                        if (string.IsNullOrEmpty(country)) {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop)).Take(n);
                        }
                        else {
                            if (string.IsNullOrEmpty(city)) {
                                tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) && x.Coutry.Contains(country)).Take(n);
                            }
                            else {
                                tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) && x.Coutry.Contains(country) && x.City.Contains(city)).Take(n);
                            }
                        }
                    }
                    else {
                        if (string.IsNullOrEmpty(country)) {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus));
                        }
                        else {
                            if (string.IsNullOrEmpty(city)) {
                                tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && x.Coutry.Contains(country));
                            }
                            else {
                                tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && x.Coutry.Contains(country) && x.City.Contains(city));
                            }
                        }
                    }
                }
                else {
                    if (subgroups.Any()) {
                        if (string.IsNullOrEmpty(country)) {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop)).Take(n);
                        }
                        else {
                            if (string.IsNullOrEmpty(city)) {
                                tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) && x.Coutry.Contains(country)).Take(n);
                            }
                            else {
                                tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) && x.Coutry.Contains(country) && x.City.Contains(city)).Take(n);
                            }
                        }
                    }
                    else {
                        if (string.IsNullOrEmpty(country)) {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus)).Take(n);
                        }
                        else {
                            if (string.IsNullOrEmpty(city)) {
                                tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && x.Coutry.Contains(country)).Take(n);
                            }
                            else {
                                tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && x.Coutry.Contains(country) && x.City.Contains(city)).Take(n);
                            }
                        }
                    }
                }

                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);

                foreach (var task in tasks) {
                    var appTask = (ApplicationTask)task;
                    var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    if (customer == null) continue;
                    appTask.Customer = (ApplicationCustomer)customer;
                    appTask.Attachments = _tasksAttachment.GetFiltered(x => x.BelongsToTask == task.ID).Select(att => (ApplicationTaskAttachment)att).ToList();
                    if (userPk != Guid.Empty) {
                        if (user != null) {
                            appTask.AlreadyBided = _taskBids.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID) != 0;
                        }
                    }
                    result.Add(appTask);
                }
                return new OperationResult<IEnumerable<ApplicationTask>>(result);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTask>>(ex);
            }
        }



        public OperationResult<bool> MyOrderCheckIsEditable(Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<bool>(false); {
                    var bidsCount = _taskBids.Count(x => x.BelongsToTask == task.ID);
                    return bidsCount == 0 && 
                        task.ProccessStatus == TaskStatus.AwaitingModeratorDecision.ToString() ||
                        task.ProccessStatus == TaskStatus.Open.ToString() ||
                        task.ProccessStatus == TaskStatus.Deactivated.ToString() ||
                        task.ProccessStatus == TaskStatus.Expired.ToString() ? 
                        new OperationResult<bool>(true) : 
                        new OperationResult<bool>(false);
                }
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> MyOrderEdit(Guid userPk, Guid taskPk, string title, string description, int price, string country, string city, DateTime topicalto) {
            try {
                var editable = MyOrderCheckIsEditable(taskPk);
                if (!editable.IsSuccess || !editable.Data) return new OperationResult<bool>(false);
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (task != null && user != null && task.Customer == user.ID) {
                    task.Name = title;
                    task.Description = description;
                    task.Price = Convert.ToInt32(price);
                    task.Coutry = country;
                    task.City = city;
                    task.TopicalTo = topicalto;
                    _tasks.UnitOfWork.Commit();
                }
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<Guid> AddTasks(string name, string description, string price, string country, string city, DateTime topicalto, string userid) {
            try {
                Guid customerPk;
                Guid.TryParse(userid, out customerPk);

                if (customerPk == Guid.Empty) return new OperationResult<Guid>(new Exception("AddTasks: Customer Pk does not found")); 
                var customer = _users.GetSingleOrDefault(x => x.PublicKey == customerPk);

                if (customer == null) return new OperationResult<Guid>(new Exception("AddTasks: Customer does not found"));

                var taskGuid = Guid.NewGuid();
                var task = new Tasks {
                    PublicKey = taskGuid,
                    Customer = customer.ID,
                    Name = name,
                    Description = description,
                    TopicalTo = topicalto,
                    Created = DateTime.Now,
                    Price = Convert.ToInt32(price),
                    ProccessStatus = TaskStatus.AwaitingModeratorDecision.ToString(),
                    Coutry = country,
                    City = city,
                    IsRemovedToCustomer = false,
                    IsRemovedToWorker = false
                };
                _tasks.Add(task);
                _tasks.UnitOfWork.Commit();
                return new OperationResult<Guid>(taskGuid);
            }
            catch(Exception ex) {
                return new OperationResult<Guid>(ex);
            }
        }

        public ApplicationTask ConfirmTask(Guid id, string group, string subgroup) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == id);
                task.ProccessStatus = TaskStatus.Open.ToString();
                task.Groop = group;
                task.SubGroop = subgroup;
                _tasks.UnitOfWork.Commit();
                return (ApplicationTask)task;
            }
            catch {
                return null;
            }
        }


        public OperationResult<IEnumerable<ApplicationTask>> GetMyOrders(Guid userId) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                if (user == null) return new OperationResult<IEnumerable<ApplicationTask>>(new Exception("GetMyOrders: user does not found"));
                var tasks = _tasks.GetFiltered(x => x.Customer == user.ID);

                if (CheckTasksIsExpired(ref tasks)) _tasks.UnitOfWork.Commit();

                var applicationTasks = new List<ApplicationTask>();

                foreach (var task in tasks) {
                    if (task.IsRemovedToCustomer == null || task.IsRemovedToCustomer == false) {
                        var worker = _users.GetSingleOrDefault(x => x.ID == task.Worker);
                        var applicationTask = (ApplicationTask)task;

                        applicationTask.Worker = new ApplicationWorker {
                            Name = worker?.Name + " " + worker?.SurName,
                            Rating = worker?.Rating,
                            UserLogoPath = worker?.UserLogoPath
                        };
                        var bids = _taskBids.GetFiltered(x => x.BelongsToTask == task.ID);
                        if (bids != null && bids.Any()) {
                            applicationTask.Biders = (from bid in bids
                                                      select _users.GetSingleOrDefault(x => x.ID == bid.BelongsToWorker)
                                                      into bider
                                                      where bider != null
                                                      select (ApplicationWorker)bider).ToList();
                        }
                        applicationTasks.Add(applicationTask);
                    }
                }
                applicationTasks = applicationTasks.OrderBy(x => x.Created).ToList();
                return new OperationResult<IEnumerable<ApplicationTask>>(applicationTasks);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTask>>(ex);
            }
        }


        public OperationResult<IEnumerable<ApplicationTask>> GetMyJobs(Guid userId) {
            try {
                var applicationTasks = new List<ApplicationTask>();
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                if (user == null) return new OperationResult<IEnumerable<ApplicationTask>>(new Exception("GetMyOrders: user does not found"));

                var myBids = _taskBids.GetFiltered(x => x.BelongsToWorker == user.ID).ToList();
                foreach (var bid in myBids){
                    var task = _tasks.GetSingleOrDefault(x => x.ID == bid.BelongsToTask);
                    var custom = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    if (task != null && custom!=null){
                        var aTask = (ApplicationTask) task;
                        aTask.BidInfo = (ApplicationTaskBid) bid;
                        aTask.Customer = (ApplicationCustomer) custom;
                        applicationTasks.Add(aTask);
                    }
                }
                return new OperationResult<IEnumerable<ApplicationTask>>(applicationTasks);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTask>>(ex);
            }
        }


        public OperationResult<bool> MyOrderRemove(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("MyOrderRemove: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk && x.Customer == user.ID);
                if (task == null) return new OperationResult<bool>(new Exception("MyOrderRemove: order does not found"));
                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus == TaskStatus.Closed) {
                    task.IsRemovedToCustomer = true;
                    _tasks.UnitOfWork.Commit();
                    return new OperationResult<bool>(true);
                }
                if (taskStatus == TaskStatus.Expired || taskStatus == TaskStatus.AwaitingModeratorDecision || 
                    (taskStatus == TaskStatus.Open && MyOrderCheckIsEditable(taskPk).Data)) {
                    task.ProccessStatus = TaskStatus.Deactivated.ToString();
                    task.IsRemovedToCustomer = true;
                    _tasks.UnitOfWork.Commit();
                    return new OperationResult<bool>(true);
                }
                return new OperationResult<bool>(false);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> MyOrderReopen(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("MyOrderReopen: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk && x.Customer == user.ID);
                if (task == null) return new OperationResult<bool>(new Exception("MyOrderReopen: order does not found"));
                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus != TaskStatus.Expired) return new OperationResult<bool>(false);
                task.ProccessStatus = TaskStatus.Open.ToString();
                _tasks.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> MyOrderClose(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("MyOrderClose: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk && x.Customer == user.ID);
                if (task == null) return new OperationResult<bool>(new Exception("MyOrderClose: order does not found"));
                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus == TaskStatus.InProgress) {
                    if (task.Customer == user.ID) {
                        task.ProccessStatus = TaskStatus.AwaitingWorkerDecision2.ToString();
                    }
                    else if (task.Worker == user.ID) {
                        task.ProccessStatus = TaskStatus.AwaitingCustomerDecision.ToString();
                    }
                    _tasks.UnitOfWork.Commit();
                }
                else if (taskStatus == TaskStatus.AwaitingCustomerDecision || taskStatus == TaskStatus.AwaitingWorkerDecision2) {
                    task.ProccessStatus = TaskStatus.Closed.ToString();
                    _tasks.UnitOfWork.Commit();
                    return new OperationResult<bool>(true);
                }
                return new OperationResult<bool>(false);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> MyOrderToDisput(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("MyOrderToDisput: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk && x.Customer == user.ID);
                if (task == null) return new OperationResult<bool>(new Exception("MyOrderToDisput: order does not found"));
                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus != TaskStatus.AwaitingCustomerDecision) return new OperationResult<bool>(false);
                task.ProccessStatus = TaskStatus.InDispute.ToString();
                _tasks.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<IEnumerable<ApplicationTask>> GetTasksAwaitedModerators() {
            try {
                var applicationTasks = new List<ApplicationTask>();
                var tasks = _tasks.GetAll(x => x.ProccessStatus == TaskStatus.AwaitingModeratorDecision.ToString()).ToList();
                foreach (var task in tasks) {
                    var appTask = (ApplicationTask) task;
                    var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    appTask.Customer = (ApplicationCustomer)customer;
                    applicationTasks.Add(appTask);
                }
                return new OperationResult<IEnumerable<ApplicationTask>>(applicationTasks);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTask>>(ex);
            }
        }


        public OperationResult<bool> TaskChecked(Guid pk, string name, string description, string group, string subGroup, int price, bool success) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == pk);
                if (task == null) return new OperationResult<bool>(new Exception("[ImpTask]/[TaskChecked] Task with Pk " + pk + "does not found"));

                task.Description = description;
                task.Name = name;
                task.Groop = group;
                task.SubGroop = subGroup;
                task.Price = price;
                task.ProccessStatus = success ? TaskStatus.Open.ToString() : TaskStatus.Deactivated.ToString();
                _tasks.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<object> CountIncompleteTasks(Guid userPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) throw new Exception("[ImpTasks]/[CountIncompleteTasks] user with pk: " + userPk + " doesnt found.");
                var incompleteTasks = _tasks.GetFiltered(x => x.Customer == user.ID &&
                    (x.ProccessStatus == TaskStatus.Open.ToString() ||
                     x.ProccessStatus == TaskStatus.Expired.ToString() ||
                     x.ProccessStatus == TaskStatus.Deactivated.ToString() ||
                     x.ProccessStatus == TaskStatus.InDispute.ToString() ||
                     x.ProccessStatus == TaskStatus.InProgress.ToString() ||
                     x.ProccessStatus == TaskStatus.AwaitingCustomerDecision.ToString()
                    ));
                var incompleteTasksPositive = 0;
                var incompleteTasksNegative = 0;
                foreach (var i4 in incompleteTasks){
                    if (i4.ProccessStatus == TaskStatus.Open.ToString() || i4.ProccessStatus == TaskStatus.InProgress.ToString() || i4.ProccessStatus == TaskStatus.AwaitingCustomerDecision.ToString()) {
                        incompleteTasksPositive++;
                    }
                    else {
                        incompleteTasksNegative++;
                    }
                }
                return new OperationResult<object>(new { IncompleteTasksPositive = incompleteTasksPositive, IncompleteTasksNegative = incompleteTasksNegative });
            }
            catch (Exception ex) {
                return new OperationResult<object>(ex);
            }
        }


        public OperationResult<object> CountIncompleteJobs(Guid userPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) throw new Exception("[ImpTasks]/[CountIncompleteTasks] user with pk: " + userPk + " doesnt found.");
                var incompleteTasks = _tasks.GetFiltered(x => x.Worker == user.ID &&
                    (x.ProccessStatus == TaskStatus.Open.ToString() ||
                     x.ProccessStatus == TaskStatus.Expired.ToString() ||
                     x.ProccessStatus == TaskStatus.Deactivated.ToString() ||
                     x.ProccessStatus == TaskStatus.InDispute.ToString() ||
                     x.ProccessStatus == TaskStatus.InProgress.ToString() ||
                     x.ProccessStatus == TaskStatus.AwaitingCustomerDecision.ToString()
                    ));
                var incompleteJobsPositive = 0;
                var incompleteJobsNegative = 0;
                foreach (var i4 in incompleteTasks) {
                    if (i4.ProccessStatus == TaskStatus.Open.ToString() || i4.ProccessStatus == TaskStatus.InProgress.ToString() || i4.ProccessStatus == TaskStatus.AwaitingCustomerDecision.ToString()) {
                        incompleteJobsPositive++;
                    }
                    else {
                        incompleteJobsNegative++;
                    }
                }
                return new OperationResult<object>(new { IncompleteJobsPositive = incompleteJobsPositive, IncompleteJobsNegative = incompleteJobsNegative });
            }
            catch (Exception ex) {
                return new OperationResult<object>(ex);
            }
        }


        // PRIVATE BLOCK
        private bool CheckTasksIsExpired(ref IEnumerable<Tasks> tasks) {
            try {
                var isExpired = false;
                foreach (var task in tasks) { 
                    var taskStatus = GetTaskStatus(task.ProccessStatus);
                    if (task.TopicalTo < DateTime.Now && taskStatus == TaskStatus.Open) {
                        task.ProccessStatus = TaskStatus.Expired.ToString();
                        isExpired = true;
                    }
                }
                return isExpired;
            }
            catch {
                return false;
            }
        }


        private static TaskStatus GetTaskStatus(string taskStatus) {
            return (TaskStatus)Enum.Parse(typeof(TaskStatus), taskStatus);
        }

    }
}
