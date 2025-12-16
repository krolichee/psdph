using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class ConversionContext
    {
        public IdentityMap IdentityMap { get; }
        public List<PendingGuidReference> PendingReferences { get; }
        public ConversionContext(IdentityMap identityMap, List<PendingGuidReference> pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }

        public ConversionContext()
        {
            IdentityMap = new IdentityMap();
            PendingReferences = new List<PendingGuidReference>();
        }
    }
}
