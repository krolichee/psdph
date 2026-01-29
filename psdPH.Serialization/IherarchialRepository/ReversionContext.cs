using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class ReversionContext
    {
        public IdentityMap IdentityMap { get; }                                    
        public PendingEntityReference[] PendingReferences { get; }

        public ReversionContext(IdentityMap identityMap, PendingEntityReference[] pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }
    }
}
