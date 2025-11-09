using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class PendingReference
    {
        public Guid TargetEntityGuid { get; set; }
        public Action<object> ReferenceSetter { get; set; }
    }
}
