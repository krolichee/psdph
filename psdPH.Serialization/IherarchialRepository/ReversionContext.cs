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
        public UnknownEntityReference[] PendingReferences { get; }

        public ReversionContext(IdentityMap identityMap, UnknownEntityReference[] pendingReferences)
        {
            IdentityMap = identityMap;
            PendingReferences = pendingReferences;
        }
    }
}
