using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Utils
{
    public class EnumWrapper
    {
        public Enum Value;
        public EnumWrapper(Enum value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.GetDescription();
        }
    }
}
