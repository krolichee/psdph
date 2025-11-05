using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace psdPH.Serialization
{
    class XmlForcedConverter
    {
        private static string ChangeType<T>(string xmlString) where T : class
        {
            var xDoc = XDocument.Parse(xmlString);
            xDoc.Root.Name = typeof(T).Name;
            xmlString = xDoc.ToString();
            return xmlString;
        }
        public static T Convert<T>(object obj) where T : class, new()
        {
            var dayBlob = new T();
            var resultXml = XmlSerializerHelper.GetXml(obj);
            resultXml = ChangeType<T>(resultXml);
            return XmlSerializerHelper.GetObj<T>(resultXml);
        }
    }
}
