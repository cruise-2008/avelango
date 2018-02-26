using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpTaskOffers : ITaskOffers
    {
        private readonly IRepository<TaskOffers> _offers;
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<Users> _users;


        public ImpTaskOffers(IRepository<TaskOffers> offers, IRepository<Users> users, IRepository<Tasks> tasks) {
            _offers = offers;
            _users = users;
            _tasks = tasks;
        }


        public OperationResult<bool> SetOffers(Guid workerPk, List<Guid> tasksPk, string message) {
            try {
                bool changeExist = false;
                var worker = _users.GetSingleOrDefault(x => x.PublicKey == workerPk);
                foreach (var task in tasksPk.Select(taskPk => _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk)).Where(task => worker != null && task != null)) {
                    if (_offers.GetSingleOrDefault(x => x.BelongsToWorker == worker.ID && x.BelongsToTask == task.ID) == null) {
                        var offerPk = Guid.NewGuid();
                        _offers.Add(new TaskOffers {
                            PublicKey = offerPk,
                            BelongsToTask = task.ID,
                            BelongsToWorker = worker.ID,
                            Created = DateTime.Now,
                            Message = message,
                        });
                        changeExist = true;
                    }
                }
                if(changeExist) _offers.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }

    }
}
