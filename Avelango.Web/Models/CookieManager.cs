using System;
using System.Web;
using System.Web.Security;
using Avelango.Models.Accessory;

namespace Avelango.Models
{
    public class CookieManager
    {
        private const string CookieName = ".ASPXAUTH";

        public OperationResult<bool> SetCookies(HttpResponseBase response, string userId) {
            try {
                // const int timeout = 525600;
                // var ticket = new FormsAuthenticationTicket(userId, true, timeout);
                // var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) {
                //     Expires = DateTime.Now.AddMinutes(timeout),
                //     HttpOnly = true
                // };
                // response.Cookies.Add(cookie);

                var ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddDays(30), false, "", FormsAuthentication.FormsCookiePath);
                var encTicket = FormsAuthentication.Encrypt(ticket);
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                return new OperationResult<bool>();
            }
            catch(Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> RemoveCookies(HttpResponseBase response) {
            try {
                var aspxAuth = response.Cookies?[CookieName];
                if (aspxAuth != null) aspxAuth.Expires = DateTime.Now.AddDays(-1);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<string> GetUserId(HttpRequestBase request) {
            try {
                var aspxAuth = request.Cookies?[CookieName];
                if (aspxAuth == null) return new OperationResult<string>(string.Empty);
                var ticket = FormsAuthentication.Decrypt(aspxAuth.Value);
                if (ticket == null) return null;
                var result = ticket.Expired ? null : ticket.Name;
                return new OperationResult<string>(result);
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }
    }
}