using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Utils.Setups
{
    public class EnumChooseSetup:Setup
    {
        public static ChooseSetup EnumChoose(SetupConfig config, Type @enum)
        {
            var enumValues = Enum.GetValues(@enum).Cast<Enum>();
            var options = enumValues.ToArray();
            return new ChooseSetup(config, options, FieldFunctions.EnumWrapperFunctions);
        }
    }
}
