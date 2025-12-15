using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class DtoScope
    {
        public DtoScope()
        {
            Scope = new List<Dto>();
        }

        public List<Dto> Scope { get; }
    }
}
