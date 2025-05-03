using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    class CompositionXmlDictionary
    {
        public static Dictionary<string, Type> StoT = new Dictionary<string, Type> { };
        class KV
        {
            public static KeyValuePair<string, Type> NewKV(string xmlname, Type type)
            {
                return new KeyValuePair<string, Type>(xmlname, type);
            }
            public static KeyValuePair<string, Type> NewKV(Type type)
            {
                XmlRootAttribute rootAttribute = (XmlRootAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute));
                return NewKV(rootAttribute.ElementName, type);
            }
        }

        public static void InitializeDictionary()
        {
            KeyValuePair<string, Type>[] pairs =
            {
                    KV.NewKV(typeof(Blob)),
                    KV.NewKV(typeof(PlaceholderLeaf)),
                    KV.NewKV(typeof(ImageLeaf)),
                    KV.NewKV(typeof(FlagLeaf)),
                    KV.NewKV(typeof(TextLeaf)),
                    KV.NewKV(typeof(LayerLeaf)),
                    KV.NewKV(typeof(PrototypeLeaf)),
                };
            foreach (var pair in pairs)
                StoT.Add(pair.Key, pair.Value);
        }
        public static string GetXmlName(Type type)
        {
            foreach (var kvp in StoT)
            {
                if (kvp.Value == type)
                    return kvp.Key;
            }

            throw new ArgumentException();
        }
        public static Type GetCompositionType(string xmlname)
        {
            return StoT[xmlname];
        }
    }
}
