using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Avelango.Models.Enums;

namespace Avelango.Handlers.Lang
{
    public class ErrorManeger
    {
        public static readonly Dictionary<string, object> ErrorRu = GetRs(Resources.ErrorMessages.Ru.Errors.ResourceManager);
        public static readonly Dictionary<string, object> ErrorDe = GetRs(Resources.ErrorMessages.De.Errors.ResourceManager);
        public static readonly Dictionary<string, object> ErrorEn = GetRs(Resources.ErrorMessages.En.Errors.ResourceManager);
        public static readonly Dictionary<string, object> ErrorEs = GetRs(Resources.ErrorMessages.Es.Errors.ResourceManager);
        public static readonly Dictionary<string, object> ErrorUa = GetRs(Resources.ErrorMessages.Ua.Errors.ResourceManager);
        public static readonly Dictionary<string, object> ErrorFr = GetRs(Resources.ErrorMessages.Fr.Errors.ResourceManager);

        public static string GetErrorByName(Langs.LangsEnum lang, string errorKey)
        {
            Dictionary<string, object> dict;
            switch (lang) {
                case Langs.LangsEnum.De: { dict = ErrorDe;
                    break;
                }
                case Langs.LangsEnum.Ru: { dict = ErrorRu; break;}
                case Langs.LangsEnum.Es: { dict = ErrorEs; break;}
                case Langs.LangsEnum.Ua: { dict = ErrorUa; break;}
                case Langs.LangsEnum.Fr: { dict = ErrorFr; break;}
                default: { dict= ErrorEn; break; }
            }
            var target = dict.SingleOrDefault(x => x.Key == errorKey);
            return target.Equals(default(KeyValuePair<string, object>)) ? string.Empty : target.Value.ToString();
        }

        private static Dictionary<string, object> GetRs(ResourceManager res) {
            return res.GetResourceSet(CultureInfo.CurrentUICulture, true, true).Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value);
        }
    }
}
