using System.Linq;
using System.Web.Mvc;
using Avelango.Hubs.Accessory;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class HubController : Controller
    {
        // POST: /Hub/TryToGetTaskOnModeration
        [AccessLevelModerator]
        public ActionResult TryToGetTaskOnModeration(string taskPk) {
            if (HubClient.TasksInModeration.Any(x => x.Key == taskPk)) {
                return Json(new { IsSuccess = true, IsBusy = true });
            }
            HubClient.TasksInModeration.Add(taskPk, new PrivateSession().Current.User.Pk.ToString());
            HubClient.MessageToModeratorsTaskStateChanged(taskPk);
            return Json(new { IsSuccess = true, IsBusy = false });
        }


        // POST: /Hub/TryToGetUserOnModeration
        [AccessLevelModerator]
        public ActionResult TryToGetUserOnModeration(string userPk) {
            if (HubClient.UsersInModeration.Any(x => x.Key == userPk)) {
                return Json(new { IsSuccess = true, IsBusy = true });
            }
            HubClient.UsersInModeration.Add(userPk, new PrivateSession().Current.User.Pk.ToString());
            HubClient.MessageToModeratorsUserStateChanged(userPk);
            return Json(new { IsSuccess = true, IsBusy = false });
        }
        

    }
}