using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Avelango.Web.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Pay()
        {
            var req = WebRequest.Create("https://www.liqpay.com/ru/checkout/card/380951000184");
            var resp = req.GetResponse();
            var restStream = resp.GetResponseStream();
            if (restStream == null) {
                return View("Error");
            }
            var sr = new StreamReader(restStream);
            var html = sr.ReadToEnd();
            resp.Close();
            sr.Close();

            if (string.IsNullOrEmpty(html)) {
                return View("Error");
            }
            ViewBag.LiqPay = html;
            return View();
        }


        // GET: Payment
        public ActionResult Ok()
        {
            return View();
        }


        // GET: Payment
        public ActionResult Error()
        {
            return View();
        }
    }
}