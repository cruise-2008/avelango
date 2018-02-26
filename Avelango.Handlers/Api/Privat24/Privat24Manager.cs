using Avelango.Handlers.Encryption;
using Avelango.Handlers.Http;
using Avelango.Handlers.Xml;
using Avelango.Models.Abstractions.ExternalApi.Sms;
using Avelango.Models.Accessory;
using Avelango.Models.ExternalApi.Sms.Privat24;

namespace Avelango.Handlers.Api.Privat24
{
    public class Privat24Manager : ISms
    {
        private HttpManager _httpManager;
        private const string SmsUrn = "https://api.privatbank.ua/p24api/sendsms";

        private readonly int _merchantId; // = 123821;
        private readonly string _password; // = "a6JHTcI7z4qClz3g1xF5e2YzN4KdAOwy";


        public Privat24Manager(int merchantId, string merchantPassword) {
            _merchantId = merchantId;
            _password = merchantPassword;
            _httpManager = new HttpManager();
        }

        public OperationResult<bool> SendSms(string fromMobileNumber, string toMobileNumber, string smsText) {
            var data = new requestData {
                oper = "cmt",
                payment = new requestDataPayment {
                    id = "",
                    prop = new[] {
                        new requestDataPaymentProp {name = "phone", value = "%2B38" + fromMobileNumber},
                        new requestDataPaymentProp {name = "phoneto", value = "%2B38" + toMobileNumber},
                        new requestDataPaymentProp {name = "text", value = smsText}
                    }
                },
                test = 0,
                wait = 0,
            };
            var dataSerializer = new Serializer<requestData>();
            var xmlData = dataSerializer.Serialize(data);

            xmlData = xmlData.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
            xmlData = xmlData.Replace("<requestData>", string.Empty);
            xmlData = xmlData.Replace("</requestData>", string.Empty);

            var signature = Md5.GetHashStringAsSha1(xmlData + _password);

            var req = new request {
                merchant = new requestMerchant {
                    id = _merchantId,
                    signature = signature
                },
                data = data,
                version = "1.0"
            };
            var requestSerializer = new Serializer<request>();
            var xml = requestSerializer.Serialize(req);
            //xml = xml.Replace("utf-16", "UTF-8");
            var response = _httpManager.PostXml(SmsUrn, xml);


            return new OperationResult<bool>();
        }
    }
}
