using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Avelango.Models;
using Avelango.Models.Abstractions.Db;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsers _user;


        public UsersController(IUsers user) {
            _user = user;
        }


        // GET: /Users/GetFilteredUsers
        public ActionResult GetFilteredUsers(List<Guid> viewedUsers, List<string> subgroups, double? placeLat, double? placeLng, bool isOnline, int countOfUsers) {
            viewedUsers = viewedUsers ?? new List<Guid>();
            subgroups = subgroups ?? new List<string>();
            var usersOnlineKeys = new List<Guid>();
            if (isOnline) {
                foreach (var i4 in Hubs.Accessory.HubClient.Users) {
                    Guid userPk;
                    Guid.TryParse(i4.Value, out userPk);
                    if (userPk != Guid.Empty) {
                        usersOnlineKeys.Add(userPk);
                    }
                }
            }
            var usersResult = _user.GetFilteredUsers(viewedUsers, subgroups, placeLat, placeLng, usersOnlineKeys, countOfUsers);

            foreach (var user in usersResult.Data) {
                user.UserLogoPath = string.IsNullOrEmpty(user.UserLogoPath) ? Resources.Default.Settings.DefaulLogoPath : user.UserLogoPath;
            }
            return Json(new { IsSuccess = true, Users = usersResult });
        }


        // GET: /Users/GetFullUserInfo
        public ActionResult GetFullUserInfo(Guid userPk) {
            var user = _user.GetFullUserInfo(userPk);
            user.UserLogoPath = string.IsNullOrEmpty(user.UserLogoPath) ? Resources.Default.Settings.DefaulLogoPath : user.UserLogoPath;
            return Json(new { IsSuccess = true, User = user });
        }


        // GET: /Users/GetMyOpenedTasks
        [AccessLevelAnyAutorized]
        public ActionResult GetMyOpenedTasks() {
            var tasks = _user.GetMyOpenedTasks(new PrivateSession().Current.User.Pk);
            return Json(new { IsSuccess = true, Tasks = tasks.Data });
        }


        // GET: /Users/Executors
        public ActionResult Executors() {
            return View();
        }

        public ActionResult ExecutorsCard()
        {
            return View();
        }
    }
}