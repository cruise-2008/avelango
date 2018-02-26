using System;
using Avelango.Handlers.Http;
using Avelango.Models.Abstractions.ExternalApi.Sms;
using Avelango.Models.Accessory;

namespace Avelango.Handlers.Api.AlphaSms
{
    public class AlphaSmsManager : ISms
    {
        private const string SmsUrn = "https://alphasms.ua/api/http.php";

        private readonly HttpManager _httpManager;

        private readonly string _login;
        private readonly string _password;
        private readonly string _apiKey;


        public AlphaSmsManager(string login, string password, string apikey) {
            _login = login;
            _password = password;
            _apiKey = apikey;
            _httpManager = new HttpManager();
        }


        public OperationResult<bool> SendSms(string from, string to, string sms) {
            try {
                var data = "version=http&login="+ _login + "&password="+ _password + "&key="+ _apiKey + "&command=send&from="+ from + "&to=38"+ to + "&message=" + sms;
                var result = _httpManager.Get(SmsUrn + "?" + data);
                if (result.IsSuccess && result.Data.Contains("id:")) {
                    return new OperationResult<bool>(true);
                }
                return new OperationResult<bool>(new Exception("SMS sending error"));
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
    }
}
