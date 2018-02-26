
using Avelango.Models.Accessory;

namespace Avelango.Models.Abstractions.ExternalApi.Sms
{
    public interface ISms
    {
        OperationResult<bool> SendSms(string fromMobileNumber, string toMobileNumber, string smsText);
    }
}
