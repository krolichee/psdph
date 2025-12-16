using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class Cloner
    {
        public static object Clone(object obj)
        {
            var type = obj.GetType();
            var resultXml = XmlSerializerHelper.GetXml(obj);
            return XmlSerializerHelper.GetObj(resultXml, type);
        }
    }
}
