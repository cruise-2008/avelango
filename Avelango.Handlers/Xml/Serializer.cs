using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Avelango.Handlers.Xml
{
    /// <summary>
    /// Generic Serializer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Serializer<T> where T : class {

        /// <summary>
        /// T Deserialize
        /// </summary>
        /// <param name="inputXml"></param>
        /// <returns></returns>
        public T Deserialize(string inputXml) {
            using (TextReader reader = new StringReader(inputXml)) {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)xs.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize T
        /// </summary>
        /// <param name="tobject"></param>
        /// <returns></returns>
        public string Serialize(T tobject) {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter()) {
                using (var writer = XmlWriter.Create(sww, new XmlWriterSettings { Encoding = new UTF8Encoding() })) {
                    xsSubmit.Serialize(writer, tobject, ns);
                    return sww.ToString();
                }
            }
        }
    }
}