using System;
using System.Collections.Generic;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpTaskPreWorkers : ITaskPreWorkers
    {
        private readonly IRepository<TaskPreWorkers> _taskPreWorkers;
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<Users> _users;

        public ImpTaskPreWorkers(IRepository<TaskPreWorkers> taskPreWorkers, IRepository<Users> users, IRepository<Tasks> tasks) {
            _taskPreWorkers = taskPreWorkers;
            _users = users;
            _tasks = tasks;
        }


        public OperationResult<ApplicationTask> SetTaskPreWorkers(Guid userPk, Guid taskPk, List<Guid> executorsPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ApplicationTask>(new Exception("SetTaskExecutors: user does not found"));
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk && x.Customer == user.ID);
                if (task == null) return new OperationResult<ApplicationTask>(new Exception("SetTaskExecutors: order does not found"));

                var executorsApproved = false;
                foreach (var executorPk in executorsPk) {
                    var worker = _users.GetSingleOrDefault(x => x.PublicKey == executorPk);

                    if (worker == null) continue;
                    if (_taskPreWorkers.Count(x => x.BelongsToTask == task.ID && x.BelongsToWorker == worker.ID) != 0) continue;

                    _taskPreWorkers.Add(new TaskPreWorkers {
                        Approved = DateTime.Now,
                        BelongsToTask = task.ID,
                        BelongsToWorker = worker.ID,
                        Message = string.Empty,
                        PublicKey = Guid.NewGuid(),
                    });
                    executorsApproved = true;
                }

                if (!executorsApproved) return new OperationResult<ApplicationTask>();
                _taskPreWorkers.UnitOfWork.Commit();

                var taskStatus = GetTaskStatus(task.ProccessStatus);
                if (taskStatus != TaskStatus.Open) return new OperationResult<ApplicationTask>();
                task.ProccessStatus = TaskStatus.AwaitingWorkerDecision1.ToString();
                _tasks.UnitOfWork.Commit();

                return new OperationResult<ApplicationTask>((ApplicationTask)task);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationTask>(ex);
            }
        }


        private static TaskStatus GetTaskStatus(string taskStatus) {
            return (TaskStatus)Enum.Parse(typeof(TaskStatus), taskStatus);
        }

    }
}
