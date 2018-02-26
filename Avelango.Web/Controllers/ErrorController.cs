using System.Net;
using System.Web.Mvc;

namespace Avelango.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Error(string errorData)
        {
            ViewBag.Error = errorData;
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
            return View();
        }

        public ActionResult Error403()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.Redirect;
            return View();
        }

        public ActionResult Error404()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        public ActionResult Error500()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }
    }
}