using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface IChats
    {
        OperationResult<List<ApplicationChats>> GetMyChats(Guid pk);
        OperationResult<Guid> AddChat(Guid taskPk, Guid userPk);
    }
}
