using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class Dto
    {
        public Guid Guid { get; protected set; } = Guid.NewGuid();
    }
}
