using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface ITaskAttachments
    {
        OperationResult<bool> Add(List<ApplicationTaskAttachment> files, Guid taskPk);
        OperationResult<bool> Remove(List<Guid> removedfiles, Guid taskPk);

    }
}
