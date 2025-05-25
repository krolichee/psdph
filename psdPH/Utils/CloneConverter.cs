using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace psdPH.Utils
{
    public class CloneConverter
    {
        private static string _getXml(object obj) {
            var type = obj.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, obj);
            return sb.ToString();
        }
        private static T _getObj<T>(string xmlString)where T:class
        {
            StringReader sr = new StringReader(xmlString);
            XmlSerializer serializer = new XmlSerializer(typeof(DayBlob));
            T result = serializer.Deserialize(sr) as T;
            return result;
        }
        private static string _changeType<T>(string xmlString) where T : class
        {
            var xDoc = XDocument.Parse(xmlString);
            xDoc.Root.Name = typeof(T).Name;
            xmlString = xDoc.ToString();
            return xmlString;
        }
        public static T Convert<T>(object blob)where T:class,new()
        {
            var dayBlob = new T();
            var resultXml = _getXml(blob);
            resultXml = _changeType<T>(resultXml);

            return _getObj<T>(resultXml);
        }
    }
}
