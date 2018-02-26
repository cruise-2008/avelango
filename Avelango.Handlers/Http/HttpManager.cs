using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avelango.Models.Accessory;

namespace Avelango.Handlers.Http
{
    public class HttpManager
    {
        public OperationResult<string> Get(string url) {
            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream == null) return new OperationResult<string>(new Exception("[HttpManager]/[Get]: Empty response Stream"));
                var responseString = new StreamReader(responseStream).ReadToEnd();
                return new OperationResult<string>(responseString);
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }

        public OperationResult<string> Post(string url, Dictionary<string, string> data) {
            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var postData = data.Aggregate(string.Empty, (current, i4) => current + i4.Key + "=" + i4.Value + "&").TrimEnd('&');
                var encodeddata = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = encodeddata.Length;

                using (var stream = request.GetRequestStream()) {
                    stream.Write(encodeddata, 0, encodeddata.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();

                var responseStream = response.GetResponseStream();
                if (responseStream == null) return new OperationResult<string>(new Exception("[HttpManager]/[Post]: Empty response Stream"));
                var responseString = new StreamReader(responseStream).ReadToEnd();
                return new OperationResult<string>(responseString);
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }


        public OperationResult<string> PostXml(string url, string requestXml) {
            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var bytes = Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return null;
                var responseStream = response.GetResponseStream();
                if (responseStream == null) return null;
                var responseStr = new StreamReader(responseStream).ReadToEnd();
                return new OperationResult<string>(responseStr);
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }


        public async Task<string> GetAsync(string url) {
            using (var client = new HttpClient()) {
                return await client.GetStringAsync(url);
            }
        }


        public async Task<string> PostAsync(string url, Dictionary<string, string> data) {
            using (var client = new HttpClient()) {
                var content = new FormUrlEncodedContent(data);
                var response = await client.PostAsync(url, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
