using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Avelango.NunitTests
{
    public static class HttpHandler
    {
        public static string Get(string url) {
            using (var client = new WebClient()) {
                var responseString = client.DownloadString(url);
                return responseString;
            }
        }


        // var values = new NameValueCollection { ["t1"] = "hello", ["t2"] = "world" };
        public static string Post(string url, NameValueCollection values) {
            using (var client = new WebClient()) {
                var response = client.UploadValues(url, values);
                var responseString = Encoding.Default.GetString(response);
                return responseString;
            }
        }
    }
}
