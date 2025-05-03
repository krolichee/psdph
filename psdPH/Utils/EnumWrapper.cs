using psdPH.Logic;
using System;

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
