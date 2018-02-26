using System;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpMail : IMails
    {
        private readonly IRepository<Mails> _mails;

        public ImpMail(IRepository<Mails> users) {
            _mails = users;
        }

        public void SaveEmailInfo(string recipient, string subject) {
            _mails.Add(new Mails {
                EmailSubject = subject,
                Recipient = recipient,
                Datetime = DateTime.Now
            });
            _mails.UnitOfWork.Commit();
        }
    }
}
