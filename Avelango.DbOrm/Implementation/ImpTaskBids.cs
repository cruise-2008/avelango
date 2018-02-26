using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpTaskBids : ITaskBids
    {
        private readonly IRepository<TaskBids> _taskBids;
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<Users> _users;


        public ImpTaskBids(IRepository<TaskBids> taskBids, IRepository<Users> users, IRepository<Tasks> tasks) {
            _taskBids = taskBids;
            _users = users;
            _tasks = tasks;
        }


        public OperationResult<ApplicationTask> BidTask(Guid taskPk, Guid userPk, string message, int price) {
            try {
                var executor = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (executor == null)
                    return new OperationResult<ApplicationTask>(new Exception("[ImpTaskBids]/[BidTask] User with PublicKey: " + userPk + " does not found."));
                if (task == null)
                    return new OperationResult<ApplicationTask>(new Exception("[ImpTaskBids]/[BidTask] Task with PublicKey: " + taskPk + " does not found."));
                _taskBids.Add(new TaskBids {
                    BelongsToTask = task.ID,
                    BelongsToWorker = executor.ID,
                    Created = DateTime.Now,
                    Denied = false,
                    Message = message,
                    Price = price,
                    PublicKey = Guid.NewGuid()
                });
                _taskBids.UnitOfWork.Commit();

                var atask = (ApplicationTask) task;
                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                atask.Customer = customer == null ? null : (ApplicationCustomer)customer;
                return new OperationResult<ApplicationTask>(atask);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }


        public OperationResult<ApplicationTask> RemoveBid(Guid taskPk, Guid userPk) {
            try {
                var executor = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (executor == null) return new OperationResult<ApplicationTask>(new Exception("[ImpTaskBids]/[RemoveBid] User with PublicKey: " + userPk + " does not found."));
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("[ImpTaskBids]/[RemoveBid] Task with PublicKey: " + taskPk + " does not found."));
                var removedtask = _taskBids.GetSingleOrDefault(x => x.BelongsToWorker == executor.ID);
                _taskBids.Remove(removedtask);
                _taskBids.UnitOfWork.Commit();
                return new OperationResult<ApplicationTask>((ApplicationTask)task);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }


        public OperationResult<List<ApplicationTaskBider>> GetTaskBiders(Guid userPk, Guid taskPk) {
            try {
                var customer = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);

                if (customer == null) return new OperationResult<List<ApplicationTaskBider>>(new Exception("[ImpTaskBids]/[GetTaskBiders] User with PublicKey: " + userPk + " does not found."));
                if (customer.PublicKey != userPk) { throw new Exception("User doesn't have required permissions."); }

                if (task == null) return new OperationResult<List<ApplicationTaskBider>>(new Exception("[ImpTaskBids]/[GetTaskBiders] Task with PublicKey: " + taskPk + " does not found."));

                var bids = _taskBids.GetFiltered(x => x.BelongsToTask == task.ID);

                var taskBiders = bids.Select(bid => new ApplicationTaskBider {
                    Bid = (ApplicationTaskBid) bid,
                    Worker = (ApplicationWorker) _users.GetSingleOrDefault(x => x.ID == bid.BelongsToWorker)
                }).ToList();

                return new OperationResult<List<ApplicationTaskBider>>(taskBiders);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationTaskBider>>(ex);
            }
        }
    }
}
