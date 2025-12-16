using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    /// <summary>
    /// Used by deserialization
    /// </summary>
    public class PendingEntityReference
    {
        public Guid TargetEntityGuid { get; set; }
        public Action<object> ReferenceSetter { get; set; }
    }
}
