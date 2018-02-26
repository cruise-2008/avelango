using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avelango.Handlers.String
{
    public static class StringManager
    {
        public static string BuildString(List<string> strs) {
            var sb = new StringBuilder();
            foreach (var str in strs) {
                sb.Append(str);
            }
            return sb.ToString();
        }

        public static string CleanFromXmlTags(string str) {
            var strs = str.Split('\n');
            var sb = new StringBuilder();
            foreach (var s in strs.Where(s => !s.Contains("?xml") && !s.Contains("!DOCTYPE") && !s.Contains("!html"))) {
                sb.Append(s);
            }
            return sb.ToString();
        }
    }
}
