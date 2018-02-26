using System;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpEvents : IEvents
    {
        private readonly IRepository<Events> _events;

        public ImpEvents(IRepository<Events> events)
        {
            _events = events;
        }


        public void Save(Guid userPk, string name, string decrtiption) {
            _events.Add(new Events {
                Datetime = DateTime.Now,
                Name = name,
                Decrtiption = decrtiption,
                UserPk = userPk
            });
            _events.UnitOfWork.Commit();
        }

        public void RemoveOutdated() {
            var oldEvents = _events.GetFiltered(x => x.Datetime < DateTime.Now.AddDays(-60));
            foreach (var oldEvent in oldEvents) {
                _events.Remove(oldEvent);
            }
            _events.UnitOfWork.Commit();
        }
    }
}
