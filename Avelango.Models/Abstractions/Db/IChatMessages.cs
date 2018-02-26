using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Orm;
using Avelango.Models.User;

namespace Avelango.Models.Abstractions.Db
{
    public interface IChatMessages
    {
        OperationResult<List<ChatMessages>> GetChatMessages(Guid chatPk);
        OperationResult<string> SaveMessage(Guid chatPk, string text, ImagePair attachment);
        OperationResult<long> CountNewMessages(Guid userPk);

    }
}
