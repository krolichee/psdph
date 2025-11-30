using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class SerializationContext
    {
        public IdentityMap IdentityMap { get; }
        public List<UnknownGuidReference> PendingReferences { get; }
        public SerializationContext(IdentityMap identityMap, List<UnknownGuidReference> pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }

        public SerializationContext()
        {
            IdentityMap = new IdentityMap();
            PendingReferences = new List<UnknownGuidReference>();
        }
    }
}
