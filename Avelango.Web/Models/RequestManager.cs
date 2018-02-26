using System.Web;

namespace Avelango.Models
{
    public class RequestManager
    {
        /// <summary>
        /// Is Reloading (Request from browser)
        /// </summary>
        /// <param name="request">HttpRequestBase object</param>
        /// <returns>bool</returns>
        public bool IsReloading(HttpRequestBase request) {
            if (request == null) return true;
            if (request.Headers["Pragma"] == "no-cache") return true;
            return request.Headers["Cache-Control"] == "max-age=0";
        }
    }
}