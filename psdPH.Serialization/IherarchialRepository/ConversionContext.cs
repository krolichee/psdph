using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class ConversionContext
    {
        public IdentityMap IdentityMap;
        public PendingReference[] PendingReferences;

        public ConversionContext(IdentityMap identityMap, PendingReference[] pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }
    }
}
