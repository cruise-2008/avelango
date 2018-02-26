using System;
using System.Web.Script.Serialization;

namespace Avelango.Handlers.Json
{
    public static class Convert<T> {

        public static T ToObject(string json) {
            return string.IsNullOrEmpty(json) ? 
                Activator.CreateInstance<T>() : 
                new JavaScriptSerializer().Deserialize<T>(json);
        }

        public static string ToJson(T obj) {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}
