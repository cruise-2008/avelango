using System;
using System.Collections.Generic;
using Avelango.Models.Application;
using Avelango.Models.Orm;

namespace Avelango.Models.Abstractions.Db
{
    public interface IRialtos
    {
        void AddUserIfNotExist(Guid userPk);
        Tuple<double, List<ApplicationRialtoUserAssets>> Bid(Guid userPk, double value);
        ApplicationRialtoState GetCommonData(Guid userPk);
        List<KeyValuePair<int, double>> GetChartData();
        string GetChampionName();
        double CloseBid(Guid userPk, Guid bidId);

        RialtoChat AddChatMessage(string userName, string message);
        List<RialtoChat> GetChatMessages();
    }
}
