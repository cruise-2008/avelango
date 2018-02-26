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
    public class ImpChats : IChats
    {
        private readonly IRepository<Chats> _chats;
        private readonly IRepository<Users> _users;
        private readonly IRepository<Tasks> _tasks;

        public ImpChats(IRepository<Chats> chats, IRepository<Users> users, IRepository<Tasks> tasks) {
            _chats = chats;
            _users = users;
            _tasks = tasks;
        }


        public OperationResult<Guid> AddChat(Guid taskPk, Guid userPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<Guid>(new Exception("AddChat: task with Pk: " + taskPk + " doesnt found."));

                var customer = _users.GetSingleOrDefault(x => x.ID == task.Customer);
                var worker = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (customer == null || worker == null) return new OperationResult<Guid>(new Exception("AddChat: User1 or User2 does not found - " + task.Customer + ", " + userPk));

                var chatExist = _chats.GetSingleOrDefault(x => (x.BelongsToUserA == customer.ID && x.BelongsToUserB == worker.ID) || 
                                                               (x.BelongsToUserA == worker.ID && x.BelongsToUserB == customer.ID));

                if (chatExist != null) return new OperationResult<Guid>();
                var chatPk = Guid.NewGuid();
                _chats.Add(new Chats {
                    BelongsToUserA = customer.ID,
                    BelongsToUserB = worker.ID,
                    PublicKey = chatPk,
                    IsBidirectional = false
                });
                _chats.UnitOfWork.Commit();
                return new OperationResult<Guid>(chatPk);
            }
            catch (Exception ex) {
                return new OperationResult<Guid>(ex);
            }
        }


        public OperationResult<List<ApplicationChats>> GetMyChats(Guid pk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == pk);
                if (user == null) return new OperationResult<List<ApplicationChats>>(new Exception("GetMyChats: User does not found"));
                var chats = _chats.GetFiltered(x => x.BelongsToUserA == user.ID || x.BelongsToUserB == user.ID);
                var appChats = new List<ApplicationChats>();
                foreach (var chat in chats) {
                    var colocutorId = chat.BelongsToUserA == user.ID ? chat.BelongsToUserB : chat.BelongsToUserA;
                    var colocutor = _users.GetSingleOrDefault(x => x.ID == colocutorId);
                    if (colocutor != null) {
                        var appChat = new ApplicationChats {
                            PublicKey = chat.PublicKey,
                            Collocutor = (ApplicationChatColocutor) (ApplicationUser) colocutor,
                            LastAction = chat.LastAction,
                            IsBidirectional = chat.IsBidirectional
                        };
                        appChats.Add(appChat);
                    }
                }
                return new OperationResult<List<ApplicationChats>>(appChats);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationChats>>(ex);
            }
        }
    }
}
