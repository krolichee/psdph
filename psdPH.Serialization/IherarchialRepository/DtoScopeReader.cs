using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class DtoScopeReader
    {
        public static DtoScope ReadScope(string path)
        {
            DtoScope scope = XmlSerializerHelper.GetObj<DtoScope>(path);
            return scope;
        }
    }
}
