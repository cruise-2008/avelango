using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Avelango.Handlers.Phone
{
    public static class PhoneManager
    {
        private static readonly Dictionary<string, object> Phones = GetRs();

        public static string GetPhoneCodeByCountryCode(string countryCode) {
            if (string.IsNullOrEmpty(countryCode)) return string.Empty;
            foreach (var phone in Phones.Where(phone => phone.Key == countryCode.ToUpper())) {
                return (string)phone.Value;
            }
            return string.Empty;
        }

        public static Dictionary<string, object> GetRs() {
            var phones = Resources.PhoneCodes.PhoneCodes.ResourceManager;
            return phones.GetResourceSet(CultureInfo.CurrentUICulture, true, true).Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value);
        }
    }
}