using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class DtoScopeRepository
    {
        public static DtoScope ReadScope(string path)
        {
            DtoScope scope = DiskOperations.LoadXml<DtoScope>(path);
            return scope;
        }
        public static void WriteScope(string path, DtoScope dtoScope)
        {
            DiskOperations.SaveXml(path, dtoScope);
        }
    }
}
