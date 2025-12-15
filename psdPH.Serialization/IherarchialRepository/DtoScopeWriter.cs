using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    static class DtoScopeWriter
    {
        public static void WriteScope(string path,DtoScope dtoScope)
        {
            DiskOperations.SaveXml(path, dtoScope);
        }
    }
}
