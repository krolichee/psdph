using psdPH.Logic;
using psdPH.Reflection;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Utils.Setups
{
    public class EnumChooseSetup:Setup
    {
        public static FieldFunctions EnumWrapperFunctions => new FieldFunctions()
        {
            ConvertFunction = (
            o => new EnumWrapper(o as Enum)
            ),
            RevertFunction = (o) => (o as EnumWrapper).Value
        };
        public static ChooseSetup EnumChoose(ReflectionConfig config, Type @enum)
        {
            var enumValues = Enum.GetValues(@enum).Cast<Enum>();
            var options = enumValues.ToArray();
            return new ChooseSetup(config, options, EnumWrapperFunctions);
        }
    }
}
