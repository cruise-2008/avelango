using System;
using System.Web.Mvc;
using Avelango.Models.Abstractions.Db;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotifycations _notifycations;


        public NotificationsController(INotifycations notifycations) {
            _notifycations = notifycations;
        }


        // GET: /Notifications/GetMyNotifications
        [AccessLevelAnyAutorized]
        public ActionResult GetMyNotifications(bool justActual) {
            var notificationResult = _notifycations.GetMyNotifications(new PrivateSession().Current.User.Pk, justActual);
            if(!notificationResult.IsSuccess) return Json(new { IsSuccess = false });
            return Json(new { IsSuccess = true, Notifications = notificationResult.Data });
        }



        // GET: /Notifications/SetNotificationAsViewed
        [AccessLevelAnyAutorized]
        public ActionResult SetProposalNotificationsAsViewed() {
            var notificationSetResult = _notifycations.SetProposalNotificationsAsViewed(new PrivateSession().Current.User.Pk);
            return Json(!notificationSetResult.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
        }


        // GET: /Notifications/SetTaskNotificationAsViewed
        [AccessLevelAnyAutorized]
        public ActionResult SetTaskNotificationAsViewed(Guid taskPk) {
            var notificationSetResult = _notifycations.SetTaskNotificationAsViewed(new PrivateSession().Current.User.Pk, taskPk);
            return Json(!notificationSetResult.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
        }


        // GET: /Notifications/SetMessageNotificationAsViewed
        [AccessLevelAnyAutorized]
        public ActionResult SetMessageNotificationAsViewed(Guid userPk) {
            var notificationSetResult = _notifycations.SetMessageNotificationAsViewed(new PrivateSession().Current.User.Pk, userPk);
            return Json(!notificationSetResult.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
        }
    }
}