using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Avelango.Handlers.Email;
using Avelango.Handlers.Lang;
using Avelango.Hubs.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Resources.Email;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    [AccessLevelModerator]
    public class ModderatorController : Controller
    {
        private readonly ITasks _task;
        private readonly IUsers _user;
        private readonly INotifycations _notifycations;


        public ModderatorController(IUsers user, ITasks task, INotifycations notifycations) {
            _user = user;
            _task = task;
            _notifycations = notifycations;
        }


        // GET: Modderator
        public ActionResult Parlour() {
            ViewData["Groups"] = new JavaScriptSerializer().Serialize(PageLangManager.GetGroupsContent(new PrivateSession().Current.CurrentLang.ToString()));
            return View();
        }


        // POST: Modderator/GetSiteActions
        public ActionResult GetSiteActions() {
            var tasksResult = _task.GetTasksAwaitedModerators();
            var usersResult = _user.GetUsersAwaitedModerators();
            return tasksResult.IsSuccess && usersResult.IsSuccess ? Json(new {
                IsSuccess = true,
                Tasks = tasksResult.Data,
                Users = usersResult.Data,
                Disputs = "",
                SuperUsers = ""
            }) : Json(new { IsSuccess = false });
        }


        // POST: Modderator/TaskChecked
        public ActionResult TaskChecked(ApplicationTask task, bool success, List<DeactivationCauses> causes) {
            var tasksResult = _task.TaskChecked(task.PublicKey, task.Name, task.Description, task.Group, task.SubGroup, task.Price, success);
            HubClient.TasksInModeration.Remove(task.PublicKey.ToString());
            var moderPk = new PrivateSession().Current.User.Pk;
            if (success) {
                _notifycations.AddNotification(NotifycationTypes.TaskConfirmed.ToString(), task.PublicKey, Guid.Parse(task.Customer.Pk), moderPk);
                HubClient.TaskConfirmed(tasksResult.Data.Customer.Pk, new JavaScriptSerializer().Serialize(new { taskPk = task.PublicKey, title = task.Name }));
            }
            else {
                _notifycations.AddNotification(NotifycationTypes.TaskDismissed.ToString(), task.PublicKey, Guid.Parse(task.Customer.Pk), moderPk);
                HubClient.TaskDismissed(tasksResult.Data.Customer.Pk, new JavaScriptSerializer().Serialize(new { taskPk = task.PublicKey, title = task.Name }));
                AlertUserToDeactivation(Guid.Parse(tasksResult.Data.Customer.Pk), causes);
            }
            return tasksResult.IsSuccess ? Json(new { IsSuccess = true }) : Json(new { IsSuccess = false });
        }


        // POST: Modderator/UserChecked
        public ActionResult UserChecked(ApplicationUser user, bool success, List<DeactivationCauses> causes) {
            var userResult = _user.UserChecked(user.Pk, success);
            HubClient.TasksInModeration.Remove(user.Pk.ToString());
            if (!success) AlertUserToDeactivation(user.Pk, causes);
            return userResult.IsSuccess ? Json(new {IsSuccess = true}) : Json(new {IsSuccess = false});
        }


        private void AlertUserToDeactivation(Guid userPk, List<DeactivationCauses> causes) {
            // Message to user -> account has been disabled
            var messageToUser = string.Empty;
            var plm = PageLangManager.GetCauses(new PrivateSession().Current.CurrentLang.ToString());

            foreach (var cause in causes) {
                switch (cause) {
                    case DeactivationCauses.BadDataFormat: {
                        messageToUser += plm.SingleOrDefault(x => x.Key == DeactivationCauses.BadDataFormat.ToString()).Value.ToString();
                        messageToUser += Environment.NewLine; break;
                    }
                    case DeactivationCauses.IncorrectFoto: {
                        messageToUser += plm.SingleOrDefault(x => x.Key == DeactivationCauses.IncorrectFoto.ToString()).Value.ToString();
                        messageToUser += Environment.NewLine; break;
                    }
                    case DeactivationCauses.ObscenityPublishing: {
                        messageToUser += plm.SingleOrDefault(x => x.Key == DeactivationCauses.ObscenityPublishing.ToString()).Value.ToString();
                        messageToUser += Environment.NewLine; break;
                    }
                    case DeactivationCauses.SuspicionBot: {
                        messageToUser += plm.SingleOrDefault(x => x.Key == DeactivationCauses.SuspicionBot.ToString()).Value.ToString();
                        messageToUser += Environment.NewLine; break;
                    }
                    case DeactivationCauses.ViolationRules: {
                        messageToUser += plm.SingleOrDefault(x => x.Key == DeactivationCauses.ViolationRules.ToString()).Value.ToString();
                        messageToUser += Environment.NewLine; break;
                    }
                }
            }
            // Deactivation Email
            var userData = _user.GetUserInfo(userPk);
            var deactivationData = new Dictionary<string, string> {{"DeactReason", messageToUser}};
            Langs.LangsEnum userLang;
            Enum.TryParse(userData.Lang, true, out userLang);
            new MailSender().Send(userData.Email, userData.Name, Connection.SenderInfo, "Deactivation", MailTypes.Deactivation, userLang, deactivationData);
        }
    }
}