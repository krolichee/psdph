using psdPH.Utils;
using System;

namespace psdPH.Logic
{
    public class FieldFunctions
    {
        public Func<object, object> ConvertFunction = (o) => (o);
        public Func<object, object> RevertFunction = (o) => (o);

        public static FieldFunctions EnumWrapperFunctions => new FieldFunctions()
        {
            ConvertFunction = (o => new EnumWrapper(o as Enum)),
            RevertFunction = (o) => (o as EnumWrapper).Value
        };
    }
}
