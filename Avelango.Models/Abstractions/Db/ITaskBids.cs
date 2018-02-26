using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface ITaskBids
    {
        OperationResult<ApplicationTask> BidTask(Guid taskPk, Guid userPk, string message, int price);
        OperationResult<ApplicationTask> RemoveBid(Guid taskPk, Guid userPk);
        OperationResult<List<ApplicationTaskBider>> GetTaskBiders(Guid userPk, Guid taskPk);
    }
}
