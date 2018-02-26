using System;
using System.Collections.Generic;
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
        private readonly IRepository<TaskPreWorkers> _taskPreWorkers;
        private readonly IRepository<TaskAttachments> _tasksAttachment;
        private readonly IRepository<Users> _users;

        public ImpTasks(IRepository<Tasks> tasks, IRepository<Users> users, IRepository<TaskAttachments> tasksAttachment,
                        IRepository<TaskBids> taskBids, IRepository<TaskPreWorkers> taskPreWorkers) {
            _tasks = tasks;
            _users = users;
            _tasksAttachment = tasksAttachment;
            _taskBids = taskBids;
            _taskPreWorkers = taskPreWorkers;
        }


        public OperationResult<ApplicationTask> GetTaskByPk(Guid userPk, Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("[ImpTasks]/[GetTaskByPk] Task with PublicKey: " + taskPk + " does not found."));
                var atask = (ApplicationTask) task;
                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                atask.Customer = (ApplicationCustomer)customer;
                atask.Attachments = _tasksAttachment.GetFiltered(x => x.BelongsToTask == task.ID).Select(att => (ApplicationTaskAttachment)att).ToList();
                if (userPk == Guid.Empty) return new OperationResult<ApplicationTask>(atask);
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user != null) atask.AlreadyBided = _taskBids.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID) != 0;
                return new OperationResult<ApplicationTask>(atask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }


        public OperationResult<List<ApplicationTask>> GetTasksByPks(Guid userPk, List<Guid> tasksPks) {
            try {
                var result = new List<ApplicationTask>();
                foreach (var taskPk in tasksPks) {
                    var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                    if (task == null) continue;

                    var atask = (ApplicationTask)task;
                    var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    atask.Customer = (ApplicationCustomer)customer;
                    atask.Attachments = _tasksAttachment.GetFiltered(x => x.BelongsToTask == task.ID).Select(att => (ApplicationTaskAttachment)att).ToList();

                    if (userPk == Guid.Empty) continue;
                    var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                    if (user != null) atask.AlreadyBided = _taskBids.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID) != 0;
                    result.Add(atask);
                }
                return new OperationResult<List<ApplicationTask>>(result);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationTask>>(ex);
            }
        }


        public OperationResult<IEnumerable<ApplicationTask>> GetFilteredTasks(Guid userPk, List<Guid> viewedTasks, List<string> subgroups, bool justActual, int n, double? placeLat, double? placeLng) {
            try {
                var states = justActual
                    ? new List<string> {TaskStatus.Open.ToString()}
                    : new List<string> {TaskStatus.Open.ToString(), TaskStatus.Expired.ToString(), TaskStatus.Closed.ToString(), TaskStatus.InProgress.ToString() };
                var result = new List<ApplicationTask>();
                IEnumerable<Tasks> tasks;
                if (viewedTasks != null && viewedTasks.Any()) {
                    if (subgroups != null && subgroups.Any()) {
                        if (placeLat != null && placeLng != null) {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) &&
                            placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(n);
                        }
                        else {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop)).Take(n);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus) &&
                            placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(n);
                        }
                        else {
                            tasks = _tasks.GetAll(x => !viewedTasks.Contains(x.PublicKey) && states.Contains(x.ProccessStatus));
                        }
                    }
                }
                else {
                    if (subgroups != null && subgroups.Any()) {
                        if (placeLat != null && placeLng != null) {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop) &&
                            placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(n);
                        }
                        else {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) && subgroups.Contains(x.SubGroop)).Take(n);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus) &&
                            placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(n);
                        }
                        else {
                            tasks = _tasks.GetAll(x => states.Contains(x.ProccessStatus)).Take(n);
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


        public OperationResult<bool> MyOrderEdit(Guid userPk, Guid taskPk, string title, string description, int price, string placeName, double? placeLat, double? placeLng, DateTime topicalto) {
            try {
                var editable = MyOrderCheckIsEditable(taskPk);
                if (!editable.IsSuccess || !editable.Data) return new OperationResult<bool>(false);
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (task != null && user != null && task.Customer == user.ID) {
                    task.Name = title;
                    task.Description = description;
                    task.Price = Convert.ToInt32(price);
                    task.PlaceName = placeName;
                    task.PlaceLat = placeLat;
                    task.PlaceLng = placeLng;
                    task.ProccessStatus = TaskStatus.AwaitingModeratorDecision.ToString();
                    task.TopicalTo = topicalto;
                    _tasks.UnitOfWork.Commit();
                }
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> MyOrderSetBiderAsExecutor(Guid executorId, Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == executorId);
                var user = _users.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null || user == null) return new OperationResult<bool>(false);
                task.Worker = user.ID;
                _tasks.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<List<ApplicationUser>> MyOrderGetBiders(Guid userPk, Guid taskPk) {
            try {
                var users = new List<ApplicationUser>();
                var task = _tasks.GetFirstOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<List<ApplicationUser>>(users); {
                    var bids = _taskBids.GetAll(x => x.BelongsToTask == task.ID).ToList();
                    if (bids.Any()) {
                        users.AddRange(from bid 
                                       in bids
                                       select _users.GetSingleOrDefault(x => x.ID == bid.BelongsToWorker) 
                                       into user
                                       where user != null
                                       select (ApplicationUser)user);
                    }
                }
                return new OperationResult<List<ApplicationUser>>(users);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationUser>>(ex);
            }
        }



        public OperationResult<Guid> AddTasks(string name, string description, string price, string placeName, double? placeLat, double? placeLng, DateTime topicalto, string userid) {
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
                    PlaceName = placeName,
                    PlaceLat = placeLat,
                    PlaceLng = placeLng,
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

        public OperationResult<ApplicationTask> ConfirmTask(Guid id, string group, string subgroup) {
            try {
                if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(subgroup))
                    return new OperationResult<ApplicationTask>(new Exception("ConfirmTask: group and subgroup can't be empty."));

                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == id);
                task.ProccessStatus = TaskStatus.Open.ToString();
                task.Groop = group;
                task.SubGroop = subgroup;
                _tasks.UnitOfWork.Commit();
                return new OperationResult<ApplicationTask>((ApplicationTask)task);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
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
                    // if (task.IsRemovedToCustomer == null || task.IsRemovedToCustomer == false) {
                        var worker = _users.GetSingleOrDefault(x => x.ID == task.Worker);
                        var applicationTask = (ApplicationTask)task;

                        applicationTask.Worker = new ApplicationWorker {
                            Name = worker?.Name + " " + worker?.SurName,
                            Rating = worker?.Rating,
                            UserLogoPath = worker?.UserLogoPath
                        };

                        var attachments = _tasksAttachment.GetFiltered(x => x.BelongsToTask == task.ID);
                        applicationTask.Attachments = attachments.Select(attachment => (ApplicationTaskAttachment)attachment).ToList();

                        var bids = _taskBids.GetFiltered(x => x.BelongsToTask == task.ID);
                        var taskBidses = bids as TaskBids[] ?? bids.ToArray();
                        if (bids != null && taskBidses.Any()) {
                            applicationTask.HasBiders = (from bid in taskBidses select _users.GetSingleOrDefault(x => x.ID == bid.BelongsToWorker) 
                                                         into bider where bider != null select (ApplicationWorker)bider).ToList().Any();
                        }
                        applicationTasks.Add(applicationTask);
                    //}
                }
                applicationTasks = applicationTasks.OrderBy(x => x.Created).ToList();
                return new OperationResult<IEnumerable<ApplicationTask>>(applicationTasks);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTask>>(ex);
            }
        }


        public OperationResult<IEnumerable<ApplicationTaskForWorker>> GetMyJobs(Guid userId) {
            try {
                var applicationTasks = new List<ApplicationTaskForWorker>();
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                if (user == null) return new OperationResult<IEnumerable<ApplicationTaskForWorker>>(new Exception("[ImpTasks]/[GetMyJobs]: user does not found"));
                var myBids = _taskBids.GetFiltered(x => x.BelongsToWorker == user.ID).ToList();
                foreach (var bid in myBids){
                    var task = _tasks.GetSingleOrDefault(x => x.ID == bid.BelongsToTask);
                    var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    if (task == null || customer == null) continue;

                    var aTask = (ApplicationTaskForWorker)task;
                    aTask.Bid = (ApplicationTaskBid)bid;
                    aTask.Customer = (ApplicationCustomer)customer;
                    aTask.ApprovedToMe = _taskPreWorkers.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID) != 0;
                    aTask.ItsMyTask = _tasks.GetSingleOrDefault(x => x.Worker == user.ID && x.PublicKey == task.PublicKey) != null;
                    applicationTasks.Add(aTask);
                }
                return new OperationResult<IEnumerable<ApplicationTaskForWorker>>(applicationTasks);
            }
            catch (Exception ex) {
                return new OperationResult<IEnumerable<ApplicationTaskForWorker>>(ex);
            }
        }


        public OperationResult<ApplicationTask> MyOrderRemove(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ApplicationTask>(new Exception("MyOrderRemove: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("MyOrderRemove: order does not found"));
                var taskStatus = GetTaskStatus(task.ProccessStatus);

                // IF CUSTOMER
                if (task.Customer == user.ID) {
                    if (taskStatus == TaskStatus.Closed) {
                        task.IsRemovedToCustomer = true;
                        _tasks.UnitOfWork.Commit();
                        return new OperationResult<ApplicationTask>();
                    }
                    if (taskStatus == TaskStatus.Expired || taskStatus == TaskStatus.AwaitingModeratorDecision || (taskStatus == TaskStatus.Open && MyOrderCheckIsEditable(taskPk).Data)) {
                        task.ProccessStatus = TaskStatus.Deactivated.ToString();
                        task.IsRemovedToCustomer = true;
                        _tasks.UnitOfWork.Commit();
                        return new OperationResult<ApplicationTask>();
                    }
                }
                // IF WORKER
                var atask = (ApplicationTask) task;
                var preWorker = _taskPreWorkers.GetSingleOrDefault(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID);
                if (preWorker != null) {
                    _taskPreWorkers.Remove(preWorker);
                    _taskPreWorkers.UnitOfWork.Commit();
                }
                var bid = _taskBids.GetSingleOrDefault(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID);
                if (bid != null) {
                    _taskBids.Remove(bid);
                    _taskBids.UnitOfWork.Commit();
                }
                if (task.Worker == user.ID) {
                    task.IsRemovedToWorker = true;
                    _tasks.UnitOfWork.Commit();
                }
                if (task.ProccessStatus == TaskStatus.Open.ToString() || (task.ProccessStatus == TaskStatus.AwaitingWorkerDecision1.ToString() && preWorker != null)) {
                    var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                    if (customer != null) atask.Customer = (ApplicationCustomer)customer;
                    return new OperationResult<ApplicationTask>(atask);
                }
                return new OperationResult<ApplicationTask>();
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
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



        public OperationResult<ApplicationTask> MyOrderClose(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ApplicationTask>(new Exception("MyOrderClose: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("MyOrderClose: order does not found"));

                var taskExist = _tasks.GetSingleOrDefault(x => x.Customer == user.ID && x.PublicKey == taskPk);
                var isCustomer = taskExist != null;
                if (taskExist == null) {
                    taskExist = _tasks.GetSingleOrDefault(x => x.Worker == user.ID && x.PublicKey == taskPk);
                    if (taskExist == null) return new OperationResult<ApplicationTask>(new Exception("MyOrderClose: user does not have any permissions to close task"));
                }
                var taskStatus = GetTaskStatus(task.ProccessStatus);
                var appTask = (ApplicationTask)task;

                var worker = _users.GetSingleOrDefault(x => x.ID == task.Worker);
                appTask.Worker = (ApplicationWorker)worker;

                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                appTask.Customer = (ApplicationCustomer)customer;

                if (isCustomer) {
                    appTask.ClosedByCustomer = true;
                    switch (taskStatus) {
                        case TaskStatus.InProgress:
                            task.ProccessStatus = TaskStatus.AwaitingWorkerDecision2.ToString();
                            appTask.ProccessStatus = TaskStatus.AwaitingWorkerDecision2.ToString();
                            break;
                        case TaskStatus.AwaitingCustomerDecision:
                            task.ProccessStatus = TaskStatus.Closed.ToString();
                            appTask.ProccessStatus = TaskStatus.Closed.ToString();
                            break;
                        default: return new OperationResult<ApplicationTask>(new Exception("MyOrderClose: order cant be closed"));
                    }
                }
                else { // Is worker
                    appTask.ClosedByCustomer = false;
                    switch (taskStatus) {
                        case TaskStatus.InProgress:
                            task.ProccessStatus = TaskStatus.AwaitingCustomerDecision.ToString();
                            appTask.ProccessStatus = TaskStatus.AwaitingCustomerDecision.ToString();
                            break;
                        case TaskStatus.AwaitingWorkerDecision2:
                            task.ProccessStatus = TaskStatus.Closed.ToString();
                            appTask.ProccessStatus = TaskStatus.Closed.ToString();
                            break;
                        default: return new OperationResult<ApplicationTask>(new Exception("MyOrderClose: order cant be closed"));
                    }
                }
                _tasks.UnitOfWork.Commit();
                return new OperationResult<ApplicationTask>(appTask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }



        public OperationResult<ApplicationTask> TaskStartWorking(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ApplicationTask>(new Exception("TaskStartWorking: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("TaskStartWorking: order does not found"));
                var preworker = _taskPreWorkers.GetSingleOrDefault(x => x.BelongsToWorker == user.ID && x.BelongsToTask == task.ID);
                if (preworker == null) return new OperationResult<ApplicationTask>(new Exception("TaskStartWorking: user does not have any permissions to change task status"));
                var bidData = _taskBids.GetSingleOrDefault(x => x.BelongsToTask == task.ID && x.BelongsToWorker == user.ID);

                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus != TaskStatus.AwaitingWorkerDecision1) return new OperationResult<ApplicationTask>(new Exception("TaskStartWorking: actual status does not allow this action"));
                task.ProccessStatus = TaskStatus.InProgress.ToString();
                task.Worker = user.ID;
                if (bidData != null) task.Price = bidData.Price;
                _tasks.UnitOfWork.Commit();

                var appTask = (ApplicationTask)task;
                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                if (customer != null) appTask.Customer = (ApplicationCustomer)customer;
                appTask.Worker = (ApplicationWorker)user;

                // add preworkers to remove button "get task" via signalR (loosers list)
                appTask.Biders = new List<ApplicationWorker>();
                var preworkers = _taskPreWorkers.GetAll(x => x.BelongsToTask == task.ID);
                foreach (var pworker in preworkers) {
                    appTask.Biders.Add(new ApplicationWorker { PublicKey = pworker.PublicKey });
                }
                return new OperationResult<ApplicationTask>(appTask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
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


        public OperationResult<ApplicationTask> TaskChecked(Guid pk, string name, string description, string group, string subGroup, int price, bool success) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == pk);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("[ImpTask]/[TaskChecked] Task with Pk " + pk + "does not found"));
                task.Description = description;
                task.Name = name;
                task.Groop = group;
                task.SubGroop = subGroup;
                task.Price = price;
                task.ProccessStatus = success ? TaskStatus.Open.ToString() : TaskStatus.Deactivated.ToString();
                _tasks.UnitOfWork.Commit();

                var atask = (ApplicationTask) task;
                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                atask.Customer = customer == null ? null : (ApplicationCustomer)customer;
                return new OperationResult<ApplicationTask>(atask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
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
