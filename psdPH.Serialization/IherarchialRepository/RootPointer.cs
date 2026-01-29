using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class RootPointer:Dto
    {
        

        public Guid RootGuid { get; set; }

        public RootPointer(Guid rootGuid)
        {
            RootGuid = rootGuid;
        }

        public RootPointer()
        {
        }

        //TODO добавить Generic
        public object GetRoot(IdentityMap identityMap)
        {
            if (identityMap == null)
                throw new ArgumentNullException();
            return identityMap.GetObject(RootGuid);
        }
        //TODO Единственное место, которое требует добавление свойста Objects у IdentityMap
        public static RootPointer FindRootPointer(DtoScope dtoScope)
        {
            if (dtoScope == null)
                throw new ArgumentNullException();
            return dtoScope.Scope.First(o=>o.GetType()==typeof(RootPointer)) as RootPointer;
        }
    }
}
