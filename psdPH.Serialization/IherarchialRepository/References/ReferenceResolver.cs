using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class ReferenceResolver
    {
        public static void ResolveReferences(ConversionContext context)
        {
            if (context == null)
                throw new ArgumentNullException();
            if (context.PendingReferences == null)
                return;
            var pReferences = context.PendingReferences;
            foreach (var pRef in pReferences)
            {
                object target = context.IdentityMap.GetObject(pRef.TargetEntityGuid);
                pRef.ReferenceSetter(target);
            }
        }
    }
}
