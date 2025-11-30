using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class RootPointer:Dto
    {
        

        Guid RootGuid { get; }

        public Guid Guid => Guid.Empty;

        public RootPointer(Guid rootGuid)
        {
            RootGuid = rootGuid;
        }
        //TODO добавить Generic
        public object GetRoot(IdentityMap identityMap)
        {
            if (identityMap == null)
                throw new ArgumentNullException();
            return identityMap.GetObject(RootGuid);
        }
        //TODO Единственное место, которое требует добавление свойста Objects у IdentityMap
        public static RootPointer FindRootPointer(IdentityMap identityMap)
        {
            if (identityMap == null)
                throw new ArgumentNullException();
            return identityMap.Objects.First(o=>o.GetType()==typeof(RootPointer)) as RootPointer;
        }
    }
}
