using System;
using System.Web.Mvc;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Web.Models;

namespace Avelango.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUsers _user;
        private readonly ILog _log;


        public HomeController(IUsers user, ILog log) {
            _user = user;
            _log = log;
        }

        public ActionResult Index() {
            try {
                var session = new PrivateSession().Current;
                if (User.Identity.IsAuthenticated && session.User == null) {
                    session.User = _user.GetUserInfo(Guid.Parse(User.Identity.Name));
                }
            }
            catch (Exception ex) {
                _log.AddError("[HomeController]/[Index]", ex.Message);
            }
            return View();
        }


        public ActionResult GroupsModal()
        {
            return View();
        }


        public ActionResult Terms() {
            return View();
        }


        public ActionResult Rules() {
            return View();
        }


        public ActionResult Contact() {
            return View();
        }


        public ActionResult About() {
            return View();
        }


        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            return View();
        }
    }
}