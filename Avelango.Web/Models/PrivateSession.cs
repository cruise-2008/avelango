using System;
using System.Collections.Generic;
using System.Web;
using Avelango.Models.Application;
using Avelango.Models.Enums;

namespace Avelango.Web.Models
{
    public class PrivateSession : IDisposable
    {
        public PrivateSession Current
        {
            get {
                var session = (PrivateSession)HttpContext.Current.Session["__PrivateSession__"];
                if (session != null) return session;
                session = new PrivateSession();
                HttpContext.Current.Session["__PrivateSession__"] = session;
                return session;
            }
        }

        public Langs.LangsEnum CurrentLang { get; set; }
        public ApplicationUser User { get; set; }
        public Guid? PublicKey { get; set; }
        public Pages.PagesEnum PageName { get; set; }
        public List<Guid> ProposalsTaskPks { get; set; }

        public string TemporyPhoneNumber { get; set; }
        public string TemporyPassword { get; set; }


        public void Dispose() {
            User = null;
            PublicKey = null;
            ProposalsTaskPks = null;
        }

    }
}
