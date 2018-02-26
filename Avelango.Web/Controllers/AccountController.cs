using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Avelango.Handlers.String;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Resources.Default;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Avelango.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsers _user;
        private readonly ILog _log;

        private static IAuthenticationManager AuthenticationManager => System.Web.HttpContext.Current.GetOwinContext().Authentication;


        public AccountController(IUsers user, ILog log) {
            _user = user;
            _log = log;
        }


        // POST: /Account/Slogin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Slogin(SocialNetworks social) {
            var res = new OperationResult<string>();
            try {
                switch (social) {
                    case SocialNetworks.Fb: { break; }
                    case SocialNetworks.Go: { break; }
                    case SocialNetworks.Ig: { break; }
                    case SocialNetworks.In: { break; }
                    case SocialNetworks.Tw: { break; }
                    case SocialNetworks.Vk: {
                        res = new Social.Vk.Auth().Authorization(); break;
                    }
                    case SocialNetworks.Yt: { break; }
                    default: return Json(new { IsSuccess = false });
                }
            }
            catch (Exception ex) {
                _log.AddError("[Account]/[Login]", ex.Message);
            }
            return res.IsSuccess ? Json(new { IsSuccess = true, Html = StringManager.CleanFromXmlTags(res.Data) }) : Json(new { IsSuccess = false, Data = res });
        }


        // POST: /Account/CheckPhone
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckPhone(string phone) {
            try {
                if (string.IsNullOrEmpty(phone) || phone.Length != 13) return Json(new { IsExist = false, SmsSended = false, LimitExceeded = false });
                var existResult = _user.CheckPhone(phone);
                if (existResult.IsSuccess && existResult.Data) {
                    return Json(new { IsExist = true, SmsSended = false, LimitExceeded = false });
                }
                // Send SMS
                var sendingEnable = _user.CheckSmsSendingEnable(phone);
                // var sendingEnable = new OperationResult<bool>(true);

                if (!sendingEnable.Data) return Json(new {IsExist = false, SmsSended = false, LimitExceeded = true});

                var password = _user.GetTempPassword();

                var passwordString = string.Join("-", Regex.Split(password.Data.ToString(), @"([0-9]{2})").Where(s => !string.IsNullOrWhiteSpace(s)).ToList());
                var sendResult = Handlers.Api.TurboSms.TurboSmsManager.Send(phone, "Your temporary password" + Environment.NewLine + passwordString);
                // var sendResult = true;

                var session = new PrivateSession().Current;
                session.TemporyPhoneNumber = phone;
                session.TemporyPassword = password.Data.ToString();

                if (!sendResult) return Json(new {IsExist = false, SmsSended = false, LimitExceeded = true});

                _user.AddSendedSms(Request.UserHostAddress, phone);
                return Json(new { IsExist = false, SmsSended = true });
            }
            catch (Exception ex) {
                _log.AddError("[Account]/[CheckPhone]", ex.Message);
            }
            return Json(new { IsExist = false });
        }


        [HttpPost]
        [AllowAnonymous]
        //[RequireHttps]
        public ActionResult Login(string phone, string password) {
            try {
                if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(password)) {
                    var user = _user.GetUserInfo(phone, password);
                    if (user == null) return Json(new { IsEnabled = false });

                    var session = new PrivateSession();
                    session.Current.User = user;
                    if (session.Current.User.IsAdmin) { session.Current.User.SubscribeToGroups = "[\"aconnect\"]"; }
                    if (session.Current.User.IsModerator) { session.Current.User.SubscribeToGroups = "[\"mconnect\"]"; }
                    SetAuth(user.Pk);

                    var imagePath = !string.IsNullOrEmpty(user.UserLogoPath) ? user.UserLogoPath : Settings.DefaulLogoPath;

                    if (user.IsAdmin) return Json(new { IsEnabled = true, Path = Settings.AdminParlourPath, ImagePath = imagePath });
                    if (user.IsModerator) return Json(new { IsEnabled = true, Path = Settings.ModeratorParlourPath, ImagePath = imagePath });
                    return user.IsEnabled ? Json(new { IsEnabled = true, ImagePath = imagePath, UserName = user.Name, SessionId = user.Pk, Groups = user.SubscribeToGroups }) : Json(new { IsEnabled = false });
                }
                return Json(new { IsEnabled = false });
            }
            catch (Exception ex) {
                _log.AddError("[Account]/[Login]", ex.Message);
            }
            return Json(new { IsEnabled = false });
        }

        // GET: /Account/Confirm
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Confirm(string tempid) {
            if (string.IsNullOrEmpty(tempid)) return RedirectToAction("Index", "Home");
            var user = _user.Confirm(Guid.Parse(tempid));
            new PrivateSession().Current.User = user;
            return RedirectToAction("IsConfirmed", "Account");
        }


        // GET: /Account/RemoveAccount
        [AllowAnonymous]
        public ActionResult RemoveAccount(string tempid) {
            _user.RemoveUser(tempid, true);
            return View();
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }


        // GET: /Account/PasswordRecovery
        [AllowAnonymous]
        public ActionResult PasswordRecovery() {
            return View();
        }


        // GET: /Account/PasswordHasBeenRecovered
        [AllowAnonymous]
        public ActionResult PasswordHasBeenRecovered() {
            return View();
        }


        // GET: /Account/PasswordRecovery
        [HttpPost]
        //[RequireHttps]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(string email) {
            var userExist = _user.CheckEmailExist(email);
            if (userExist.IsSuccess) {
                // Recovery todo:
                return Json(new { Success = true, Path = "/Account/PasswordHasBeenRecovered" });
            }
            return Json(new { Success = false });
        }



        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(string code, string name) {
            try {
                var session = new PrivateSession().Current;
                if (session.TemporyPassword != code) {
                    return Json(new { Success = false, IncorrectCode = true, UsesExist = false });
                }
                var user = new ApplicationUser { PhoneNumber = session.TemporyPhoneNumber, PasswordHash = code, Name = name };
                var userCreationResult = _user.Register(user);

                if (!userCreationResult.IsSuccess) {
                    _log.AddError("[Account]/[Register]", userCreationResult.Exception.Message);
                    return Json(new { Success = false, IncorrectCode = false, UsesExist = false });
                }

                if (userCreationResult.Data == null || !userCreationResult.IsSuccess) return Json(new {Success = false, IncorrectCode = false, UsesExist = true});

                session.Current.User = userCreationResult.Data;
                SetAuth(userCreationResult.Data.Pk);
                // Send mail
                //new Task(() => {
                //    new MailSender().Send(name, name, Connection.SenderInfo, "Registration", MailTypes.Confirm,
                //        new PrivateSession().Current.CurrentLang, new Dictionary<string, string> { { "UserKey", user.Id } });
                //    _mail.SaveEmailInfo(name, "Registration");
                //}).Start();

                // Add event
                //Task.Run(() => { _events.Save(Guid.Parse(user.Id), "Registration", "Email: " + user.Email + ", IP: " + Request.UserHostAddress); });
                return Json(new { Success = true, IsEnabled = true, ImagePath = Settings.DefaulLogoPath, UserName = userCreationResult.Data.Name, SessionId = userCreationResult.Data.Pk, Groups = string.Empty});
            }
            catch (Exception ex) {
                _log.AddError("[Account]/[Register]", ex.Message);
                return Json(new { Success = false, Path = "/Account/Register" });
            }
        }


        // GET: /Account/IsRegistered
        [AllowAnonymous]
        public ActionResult IsRegistered() {
            return View();
        }


        // GET: /Account/IsConfirmed
        [AllowAnonymous]
        public ActionResult IsConfirmed() {
            return View();
        }


        // GET: /Account/LogOff
        [AccessLevelAnyAutorized]
        public ActionResult LogOff() {
            new PrivateSession().Current.Dispose();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        private static void SetAuth(Guid pk) {
            var claim = new ClaimsIdentity(DefaultAuthenticationTypes.ExternalCookie);
            claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, pk.ToString(), ClaimValueTypes.String));
            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) }, claim);
            FormsAuthentication.SetAuthCookie(pk.ToString(), true);
        }
    }
}