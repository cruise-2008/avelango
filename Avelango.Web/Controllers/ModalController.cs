using System.Web.Mvc;

namespace Avelango.Web.Controllers
{
    public class ModalController : Controller
    {
        // GET: Modal
        public ActionResult Order()
        {
            return PartialView();
        }
    }
}