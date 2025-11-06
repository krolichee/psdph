using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class RootPointer
    {
        Guid RootGuid;
        public object GetRoot(IdentityMap identityMap)
        {
            return identityMap.GetObject(RootGuid);
        }
        //TODO Единственное место, которое требует добавление свойста Objects у IdentityMap
        public static RootPointer FindRootPointer(IdentityMap identityMap)
        {
            return identityMap.Objects.First(o=>o.GetType()==typeof(RootPointer)) as RootPointer;
        }
    }
}
