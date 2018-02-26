using Avelango.Handlers.Api.AlphaSms;
using Avelango.Handlers.Api.Privat24;
using Avelango.Models.Abstractions.ExternalApi.Sms;
using Avelango.Models.Accessory;

namespace Avelango.Handlers.Sms
{
    public class SmsSender
    {
        private ISms Sender { get; set; }


        /// <summary>
        /// Privat24 SMS API
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="merchantPassword"></param>
        public SmsSender(int merchantId, string merchantPassword) {
            Sender = new Privat24Manager(merchantId, merchantPassword);
        }


        /// <summary>
        /// Privat24 SMS API
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="apikey"></param>
        public SmsSender(string login, string password, string apikey) {
            Sender = new AlphaSmsManager(login, password, apikey);
        }


        public OperationResult<bool> Send(string fromMobileNumber, string toMobileNumber, string smsText) {
            return Sender.SendSms(fromMobileNumber, toMobileNumber, smsText);
        }
    }
}
