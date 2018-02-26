using System.Collections.Generic;
using System.Web.Mvc;
using Avelango.Models;
using Avelango.Models.Abstractions.Db;
using Avelango.Web.Models.Attributes;

namespace Avelango.web.Controllers
{
    [AccessLevelAdmin]
    public class AddminController : Controller
    {
        private readonly IUsers _user;

        public AddminController(IUsers user) {
            _user = user;
        }


        // GET: Addmin/Parlour
        public ActionResult Parlour()
        {
            ViewBag.Refresh = new RequestManager().IsReloading(Request);
            return View();
        }


        // POST: Addmin/GetUsersInfo
        public ActionResult GetUsersInfo() {
            var users = _user.GetUsersInfo();
            var listUsers = new List<object>();
            foreach (var user in users) {
                listUsers.Add(new {
                    id = user.Id,
                    firstName = user.Name,
                    lastName = user.SurName,
                    email = user.Email,
                    subscribeToGroups = user.SubscribeToGroups,
                    logo = user.UserLogoPath,
                    jobsDone = 0,
                    createdJobs = 0,
                    ratingWorker = "4.7",
                    ratingCustomer = "4.0",
                    confirmation = false,
                    rewards = "",
                    ballance = user.Ballance
                });
            }
            return Json(listUsers);
        }


        // POST: Addmin/GetModeratorsInfo
        public ActionResult GetModeratorsInfo() {
            var moderators = _user.GetModeratorsInfo();
            var listModerators = new List<object>();
            foreach (var moderator in moderators) {
                listModerators.Add(new {
                    id = moderator.Id,
                    firstName = moderator.Name,
                    lastName = moderator.SurName,
                    subscribeToGroups = moderator.SubscribeToGroups,
                    logo = moderator.UserLogoPath
                });
            }
            return Json(listModerators);
        }


        // POST: Addmin/RemoveUser
        public ActionResult RemoveUser(string id) {
            return Json(new { result = _user.RemoveUser(id, false) }); 
        }


        // POST: Addmin/CreateUser
        public ActionResult CreateUser() {
            return Json(true);
        }


        // POST: Addmin/ChangeUserData
        public ActionResult ChangeUserData() {
            return Json(true);
        }
    }
}