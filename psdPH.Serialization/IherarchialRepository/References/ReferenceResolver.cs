using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class ReferenceResolver
    {
        public static void ResolveReferences(ConversionContext context)
        {
            var pReferences = context.PendingReferences;
            foreach (var pRef in pReferences)
            {
                object target = context.IdentityMap.GetObject(pRef.TargetEntityGuid);
                pRef.ReferenceSetter(target);
            }
        }
    }
}
