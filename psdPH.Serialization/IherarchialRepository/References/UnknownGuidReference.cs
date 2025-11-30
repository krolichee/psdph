using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    /// <summary>
    /// Used by serialization
    /// </summary>
    public class UnknownGuidReference
    {
        public object TargetEntity { get; set; }
        public Action<Guid> ReferenceSetter { get; set; }
    }
}
