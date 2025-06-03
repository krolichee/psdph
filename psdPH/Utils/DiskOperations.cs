using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace psdPH.Utils
{

    static class DiskOperations
    {
        public static T OpenXml<T>(string path)where T:class
        {
            T result = default(T);
            if (File.Exists(path))
            {
                var stringXml = File.ReadAllText(path, Encoding.Unicode);
                result = CloneConverter.GetObj<T>(stringXml);
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
            try
            {
                var stringXml = CloneConverter.GetXml(obj);
                result.Serialized = true;
                File.WriteAllText(path, stringXml, Encoding.Unicode);
                result.Written = true;
            }
            catch { }
            return result;
        }
    }
}
