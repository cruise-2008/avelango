using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;

namespace Avelango.Models.Abstractions.Db
{
    public interface ICommonNews
    {
        OperationResult<bool> Add(string newsText);
        OperationResult<List<ApplicationNews>> GetAll(Langs.LangsEnum lang);
        OperationResult<bool> Remove(Guid pk);

    }
}
