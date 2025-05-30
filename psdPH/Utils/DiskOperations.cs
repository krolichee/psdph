﻿using System.IO;
using System.Xml.Serialization;

namespace psdPH.Utils
{

    static class DiskOperations
    {
        public static T OpenXml<T>(string path)
        {
            T result = default(T);
            FileStream fileStream;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (File.Exists(path))
            {
                fileStream = new FileStream(path, FileMode.Open);
                result = (T)serializer.Deserialize(fileStream);
                fileStream.Close();
            }
            return result;
        }

        public static void SaveXml<T>(string path, T obj)
        {
            FileStream fileStream;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            fileStream = new FileStream(path, FileMode.Create);
            serializer.Serialize(fileStream, obj);
            fileStream.Close();
        }
    }
}
