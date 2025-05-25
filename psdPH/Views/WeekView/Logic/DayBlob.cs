using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace psdPH.Views.WeekView.Logic
{
   public class DayBlob:Blob
    {
        public int Week=0;
        public DayOfWeek Dow=DayOfWeek.Monday;
        private static string _getBlobXml(Blob blob)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Blob));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, blob);
            return sb.ToString();
        }
        private static string _getDayBlobXml(DayBlob blob)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DayBlob));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, blob);
            return sb.ToString();
        }

        public static DayBlob FromBlob(Blob blob)
        {
            var dayBlob = new DayBlob();
            var blobXml = _getBlobXml(blob);
            var dayBlobXml = _getDayBlobXml(dayBlob);
            
            var blobXmlDoc = new XmlDocument();
            blobXmlDoc.LoadXml(blobXml);
            var dayBlobXmlDoc = new XmlDocument();
            dayBlobXmlDoc.LoadXml(dayBlobXml);

            var xDoc = XDocument.Parse(blobXml);
            xDoc.Root.Name = nameof(DayBlob); // Меняем имя корня
            blobXml = xDoc.ToString();

            StringReader sr = new StringReader(blobXml);
            XmlSerializer serializer = new XmlSerializer(typeof(DayBlob));
            DayBlob result = serializer.Deserialize(sr) as DayBlob;
            result.Restore();
            return result;
        }
    }
}
