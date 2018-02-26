using System.Web;
using System.Web.Mvc;

namespace Avelango.Web.Models.Attributes
{
    public class AccessLevelAdmin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            var sessionCurrent = new PrivateSession().Current;
            return sessionCurrent.User != null && sessionCurrent.User.IsAdmin;
        }
    }

    public class AccessLevelModerator : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            var sessionCurrent = new PrivateSession().Current;
            return sessionCurrent.User != null && sessionCurrent.User.IsModerator;
        }
    }

    public class AccessLevelAnyAutorized : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            var sessionCurrent = new PrivateSession().Current;
            return sessionCurrent.User != null && sessionCurrent.User.IsEnabled;
        }
    }

    public class BrowserRequestDenied : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Request.Headers["Cache-Control"] == null;
        }
    }
}