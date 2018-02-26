using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;
using Avelango.Models.User;

namespace Avelango.Models.Abstractions.Db
{
    public interface IPortfolios
    {
        OperationResult<bool> AddPortfolioData(Guid userId, string title, string description);
        OperationResult<string> JobUpLoad(Guid userId, string title, string description, List<ImagePair> imagePair);
        OperationResult<ApplicationPortfolio> GetPortfolio(Guid userId);
        OperationResult<ImagePair> RemovePortfolioJobImage(Guid userId, Guid jobPk, Guid imagePk);
        OperationResult<bool> RemovePortfolioJob(Guid userId, Guid jobPk);
        OperationResult<bool> AddPortfolioJobImage(Guid userId, Guid jobPk, List<ImagePair> imagePair);
        OperationResult<bool> ChangePortfolioJobData(Guid userId, Guid jobPk, string title, string description);
    }
}
