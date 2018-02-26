using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Avelango.Handlers.Email;
using Avelango.Models.Enums;
using Avelango.Resources.Email;

namespace Avelango.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: /Test/SendEmail
        [HttpGet]
        public ActionResult SendEmail( string lang, string type, string to) {
            try {
                var elang = Langs.LangsEnum.En;
                switch (lang.Trim().ToLower()) {
                    case "ru": { elang = Langs.LangsEnum.Ru; break; }
                    case "en": { elang = Langs.LangsEnum.En; break; }
                    case "de": { elang = Langs.LangsEnum.De; break; }
                    case "es": { elang = Langs.LangsEnum.Es; break; }
                    case "fr": { elang = Langs.LangsEnum.Fr; break; }
                    case "ua": { elang = Langs.LangsEnum.Ua; break; }
                }

                var reason = string.Empty;
                switch (lang.Trim().ToLower()) {
                    case "ru": { reason = Resources.Causes.Ru.Content.ObscenityPublishing; break; }
                    case "en": { reason = Resources.Causes.En.Content.ObscenityPublishing; break; }
                    case "de": { reason = Resources.Causes.De.Content.ObscenityPublishing; break; }
                    case "es": { reason = Resources.Causes.Es.Content.ObscenityPublishing; break; }
                    case "fr": { reason = Resources.Causes.Fr.Content.ObscenityPublishing; break; }
                    case "ua": { reason = Resources.Causes.Ua.Content.ObscenityPublishing; break; }
                }

                var etype = new KeyValuePair<MailTypes, Dictionary<string, string>>();
                switch (type.Trim().ToLower()) {
                    case "confirm": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.Confirm, new Dictionary<string, string> {
                            { "UserKey", "c0c803f7-9df7-4de8-9293-2562d20da89d" }
                        }); break;
                    }
                    case "deactivation": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.Deactivation, new Dictionary<string, string> {
                            { "DeactReason", reason }
                        }); break;
                    }
                    case "delete": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.Delete, new Dictionary<string, string> {
                            { "DeleteReason", reason}
                        }); break;
                    }
                    case "newtask": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.NewTask, new Dictionary<string, string> {
                            { "NewTaskHeader", "Эта какой-то хэадер задания" },
                            { "NewTaskContent", "Эта какой-то контент задания" }
                        }); break;
                    }
                    case "news": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.News, new Dictionary<string, string> {
                            { "NewsHeader", "Эта какой-то хэадер новости" },
                            { "NewsContent", "Эта какой-то контент новости" }
                        }); break;
                    }
                    case "passwordrecovery": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.PasswordRecovery, new Dictionary<string, string> ()); break;
                    }
                    case "taskresponse": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.TaskResponse, new Dictionary<string, string> {
                            { "TaskResponseHeader", "Эта какой-то хэадер ответ на задание" },
                            { "TaskResponseContent", "Эта какой-то контент ответ на задание" }
                        }); break;
                    }
                    case "warning": {
                        etype = new KeyValuePair<MailTypes, Dictionary<string, string>>(MailTypes.Warning, new Dictionary<string, string> {
                            { "WarningHeader", "Эта какой-то хэадер варнинга" },
                            { "WarningContent", "Эта какой-то контент варнинга" }
                        }); break;
                    }
                }

                new Task(() => {
                    var mailSender = new MailSender();
                    mailSender.Send(to, to, Connection.SenderInfo, elang + "-" + type, etype.Key, elang, etype.Value);
                }).Start();

                ViewBag.Lang = lang;
                ViewBag.Type = type;
                ViewBag.To = to;
                return View();
            }
            catch (Exception ex) {
                return Json(new { Done = "Man .. something went wrong!" + ex.Message });
            }
        }
    }
}