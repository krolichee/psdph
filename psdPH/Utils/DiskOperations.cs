using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace psdPH.Utils
{

    static class DiskOperations
    {
        public static T OpenXml<T>(string path)
        {
            T result = default(T);
            FileStream fileStream;
            XmlSerializer serializer = new XmlSerializer(typeof(T), KnownTypes.Types.ToArray());
            if (File.Exists(path))
            {
                fileStream = new FileStream(path, FileMode.Open);
                result = (T)serializer.Deserialize(fileStream);
                fileStream.Close();
            }
            return result;
        }
        public struct SaveXmlResult
        {
            public bool Serialized;
            public bool Written;
        }
        public static SaveXmlResult SaveXml<T>(string path, T obj)
        {
            SaveXmlResult result = new SaveXmlResult() { Serialized = false,Written = false };
            XmlSerializer serializer = new XmlSerializer(typeof(T),KnownTypes.Types.ToArray());
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            try
            {
                serializer.Serialize(sw, obj);
                result.Serialized = true;
                File.WriteAllText(path, sb.ToString(), Encoding.Unicode);
                result.Written = true;
            }
            catch { }
            return result;
           

            
        }
    }
}
