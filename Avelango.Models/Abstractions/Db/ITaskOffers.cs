using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;

namespace Avelango.Models.Abstractions.Db
{
    public interface ITaskOffers
    {
        OperationResult<bool> SetOffers(Guid userPk, List<Guid> tasksPk, string message);
    }
}
