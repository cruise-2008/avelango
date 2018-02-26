using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web.Script.Serialization;

namespace Avelango.Models
{
    public class AvelangoResourceManager
    {
        private readonly ResourceManager _en;
        private readonly ResourceManager _fr;
        private readonly ResourceManager _de;
        private readonly ResourceManager _es;
        private readonly ResourceManager _pl;
        private readonly ResourceManager _ua;
        private readonly ResourceManager _ru;
        private readonly ResourceManager _he;
        private readonly ResourceManager _by;

        public AvelangoResourceManager(ResourceManager en, ResourceManager ru, ResourceManager ua, ResourceManager by, ResourceManager fr,
                                     ResourceManager de, ResourceManager es, ResourceManager pl, ResourceManager he) {
            _en = en; _ru = ru; _ua = ua; _by = by; _fr = fr; _de = de; _es = es; _pl = pl; _he = he; 
        }

        public String GetJsonResourceSet(string lang) {
            try {
                ResourceSet resourceSet;
                switch (lang.ToLower().Trim()) {
                    case "fr": resourceSet = _fr.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "de": resourceSet = _de.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "es": resourceSet = _es.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "pl": resourceSet = _pl.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "he": resourceSet = _he.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "ru": resourceSet = _ru.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "ua": resourceSet = _ua.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    case "by": resourceSet = _by.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                    default: resourceSet = _en.GetResourceSet(CultureInfo.CurrentUICulture, true, true); break;
                }
                var resourceDictionary = resourceSet.Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value.ToString());
                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(resourceDictionary);
            }
            catch (Exception ex) {
                return "HTTP Error. Getting resource file data was failed." + Environment.NewLine + ex.Message;
            }
        }
    }
}