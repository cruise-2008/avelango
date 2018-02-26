using System.Web.Mvc;

namespace Avelango.Web.Controllers
{
    public class WizardController : Controller
    {
        // GET: Wizard/TaskWizard
        public ActionResult TaskWizard()
        {
            return PartialView();
        }

        // GET: Wizard/LoginWizard
        public ActionResult LoginWizard()
        {
            return PartialView();
        }
    }
}