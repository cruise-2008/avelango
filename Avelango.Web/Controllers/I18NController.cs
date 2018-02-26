using System.Web.Mvc;
using Avelango.Handlers.Lang;
using Avelango.Models.Enums;
using Avelango.Web.Models;

namespace Avelango.Web.Controllers
{
    public class I18NController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public void SetLang(string lang) {
            var privateSession = new PrivateSession();
            var tn = lang.ToLower().Trim();
            switch (tn) {
                case "ru": { privateSession.Current.CurrentLang = Langs.LangsEnum.Ru; break; }
                case "en": { privateSession.Current.CurrentLang = Langs.LangsEnum.En; break; }
                case "de": { privateSession.Current.CurrentLang = Langs.LangsEnum.De; break; }
                case "es": { privateSession.Current.CurrentLang = Langs.LangsEnum.Es; break; }
                case "ua": { privateSession.Current.CurrentLang = Langs.LangsEnum.Ua; break; }
                case "fr": { privateSession.Current.CurrentLang = Langs.LangsEnum.Fr; break; }
            }
        }


        // /I18N/GetContent
        [AllowAnonymous]
        public ActionResult GetContent(string lang, string page) {
            if (string.IsNullOrEmpty(lang)) lang = "en";
            if (string.IsNullOrEmpty(page)) page = string.Empty;
            var content = PageLangManager.GetPageContent(page, lang);
            return Json(content, JsonRequestBehavior.AllowGet);
        }


        // /I18N/GetGroupContent
        [AllowAnonymous]
        public ActionResult GetGroupContent(string lang) {
            if (string.IsNullOrEmpty(lang)) lang = "en";
            var content = GroupManager.GetGroupTree(lang);
            return Json(content, JsonRequestBehavior.AllowGet);
        }
    }
}