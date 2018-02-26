using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;


namespace Avelango.Models.Abstractions.Db
{
    public interface INotifycations
    {
        OperationResult<bool> AddNotification(string notifyType, Guid taskPk, Guid userPk, Guid fromUserPk);
        OperationResult<bool> AddNotifications(string notifyType, List<Guid> tasksPks, Guid userPk, Guid fromUserPk);
        OperationResult<bool> AddNotifications(string notifyType, Guid taskPk, List<Guid> usersPk, Guid fromUserPk);
        OperationResult<List<ApplicationNotifycation>> GetMyNotifications(Guid userId, bool justActual);
        OperationResult<bool> SetProposalNotificationsAsViewed(Guid userPk);
        OperationResult<bool> SetTaskNotificationAsViewed(Guid userPk, Guid taskPk);
        OperationResult<bool> SetMessageNotificationAsViewed(Guid userPk, Guid collocutorPk);
        OperationResult<List<Guid>> GetMyProposals(Guid userPk);
    }
}
