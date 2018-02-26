using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface ICommonAdvertising
    {
        OperationResult<bool> Add(DateTime expired, string companyName, int companyRate, string firstData, string secondData, string icon, string imagePath);
        OperationResult<List<ApplicationAdvertising>> GetAll();
        OperationResult<bool> Remove(Guid pk);
    }
}
