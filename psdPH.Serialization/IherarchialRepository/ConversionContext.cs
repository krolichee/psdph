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
        public PendingReference[] PendingReferences { get; }

        public ConversionContext(IdentityMap identityMap, PendingReference[] pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }
    }
}
