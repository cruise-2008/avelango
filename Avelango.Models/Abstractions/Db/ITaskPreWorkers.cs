using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface ITaskPreWorkers
    {
        OperationResult<ApplicationTask> SetTaskPreWorkers(Guid userPk, Guid taskPk, List<Guid> executorsPk);
    }
}
