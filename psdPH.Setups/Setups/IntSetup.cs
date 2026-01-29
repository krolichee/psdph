using psdPH.Logic;
using psdPH.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Setups
{
    public class IntSetup:Setup
    {
        public IntSetup(ReflectionConfig config, int? min = null, int? max = null):base(config)
        {
            var ntb = new NumericTextBox((int)config.GetValue(), min, max);
            Control = ntb;
            _stack.Children.Add(ntb);
            valueFunc = () => ntb.GetNumber();
        }
    }
}
