using System;
using System.Web.Mvc;
using Avelango.Handlers.Lang;
using Avelango.Handlers.Phone;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Enums;
using Avelango.Resources.Default;
using Avelango.Web.Models;

namespace Avelango.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUsers _user;
        private readonly ILog _log;

        public BaseController(ILog log, IUsers user) {
            _log = log;
            _user = user;
        }

        // POST: /Base/GetState
        [HttpPost]
        public ActionResult GetState() {
            try {
                var parlourPath = Settings.UserParlourPath;
                var session = new PrivateSession().Current;
                _log.AddInfo("GetState", "User: " + session.Current.User + "   IsAuthenticated: " + User.Identity.IsAuthenticated);
                if (session.User != null) {
                    if (session.User.IsAdmin) parlourPath = Settings.AdminParlourPath;
                    if (session.User.IsModerator) parlourPath = Settings.ModeratorParlourPath;
                    return Json(new {
                        IsEnabled = true,
                        IsAuthanticated = session.User.IsEnabled,
                        ImagePath = !string.IsNullOrEmpty(session.User.UserLogoPath) ? session.User.UserLogoPath : Settings.DefaulLogoPath,
                        UserName = session.User.Name,
                        ParlourPath = parlourPath,
                        SessionId = session.User.Id,
                        Groups = session.User.SubscribeToGroups
                    });
                }
                if (!User.Identity.IsAuthenticated)
                    return Json( new { IsEnabled = false, IsAuthanticated = false, ImagePath = (string) null, ParlourPath = (string) null });
                session.User = _user.GetUserInfo(Guid.Parse(User.Identity.Name));
                if (session.User.IsAdmin) parlourPath = Settings.AdminParlourPath;
                if (session.User.IsModerator) parlourPath = Settings.ModeratorParlourPath;
                return Json(new {
                    IsEnabled = true,
                    IsAuthanticated = session.User.IsEnabled,
                    ImagePath = !string.IsNullOrEmpty(session.User.UserLogoPath) ? session.User.UserLogoPath : Settings.DefaulLogoPath,
                    UserName = session.User.Name,
                    ParlourPath = parlourPath,
                    SessionId = session.User.Id,
                    Groups = session.User.SubscribeToGroups
                });
            }
            catch (Exception ex) {
                _log.AddError("GetState", ex.Message);
                return Json(new { IsEnabled = false, IsAuthanticated = false, ImagePath = (string)null, ParlourPath = (string)null });
            }
        }


        // GET: /Base/GetMyGroups
        [HttpGet]
        public ActionResult GetMyGroups() {
            try {
                var session = new PrivateSession().Current;
                var groups = PageLangManager.GetGroupsContent(session.CurrentLang.ToString());
                return Json(new { IsSuccess = true, Groups = groups }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                _log.AddError("GetMyGroups", ex.Message);
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: /Base/GetLocations
        [HttpGet]
        public ActionResult GetLocations() {
            var session = new PrivateSession().Current;
            return Json(new { IsSuccess = true, Locations = "" }, JsonRequestBehavior.AllowGet);
        }


        // GET: /Base/SetCallBack
        [HttpGet]
        public ActionResult SetCallBack(Pages.PagesEnum pageName, Guid? publicKey) {
            var ps = new PrivateSession().Current;
            ps.PageName = pageName;
            ps.PublicKey = publicKey;
            return Json(new {IsSuccess = true});
        }


        // GET: /Base/GetPhoneCode
        [HttpPost]
        public ActionResult GetPhoneCode(string countryCode) {
            return Json(new { IsSuccess = true, PhoneCode = PhoneManager.GetPhoneCodeByCountryCode(countryCode) });
        }
    }
}