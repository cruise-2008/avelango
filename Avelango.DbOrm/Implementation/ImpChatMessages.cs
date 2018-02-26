using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Orm;
using Avelango.Models.User;

namespace Avelango.DbOrm.Implementation
{
    public class ImpChatMessages : IChatMessages
    {
        private readonly IRepository<ChatMessages> _chatMessages;
        private readonly IRepository<Chats> _chats;
        private readonly IRepository<Users> _users;

        public ImpChatMessages(IRepository<ChatMessages> chatMessages, IRepository<Chats> chats, IRepository<Users> users)
        {
            _chatMessages = chatMessages;
            _chats = chats;
            _users = users;
        }


        public OperationResult<long> CountNewMessages(Guid userPk) {
            try {
                long newMessages = 0;
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<long>(newMessages); {
                    var chats = _chats.GetFiltered(x => x.BelongsToUserA == user.ID || x.BelongsToUserB == user.ID);
                    newMessages += chats.Sum(chat => _chatMessages.Count(x => x.BelongToChat == chat.ID && x.IsNew));
                }
                return new OperationResult<long>(newMessages);
            }
            catch (Exception ex) {
                return new OperationResult<long>(ex);
            }
        }


        public OperationResult<List<ChatMessages>> GetChatMessages(Guid chatPk) {
            try {
                var chat = _chats.GetSingleOrDefault(x => x.PublicKey == chatPk);
                var messages = _chatMessages.GetFiltered(x => x.BelongToChat == chat.ID).Where(x => x.Created > DateTime.Now.AddDays(-31));
                return new OperationResult<List<ChatMessages>>(messages.ToList());
            }
            catch (Exception ex) {
                return new OperationResult<List<ChatMessages>>(ex);
            }
        }


        public OperationResult<string> SaveMessage(Guid chatPk, string text, ImagePair attachment) {
            try {
                var chat = _chats.GetSingleOrDefault(x => x.PublicKey == chatPk);
                if (chat == null) return new OperationResult<string>(new Exception("SaveMessage: Chat with Pk-" + chatPk + " does not found"));
                _chatMessages.Add(new ChatMessages {
                    AttachmentMin = attachment?.Small,
                    AttachmentMax = attachment?.Large,
                    BelongToChat = chat.ID,
                    Created = DateTime.Now,
                    IsNew = true,
                    Message = text,
                    PublicKey = Guid.NewGuid()
                });
                chat.IsBidirectional = true;
                _chats.UnitOfWork.Commit();
                _chatMessages.UnitOfWork.Commit();
                return new OperationResult<string>();
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }

    }
}
