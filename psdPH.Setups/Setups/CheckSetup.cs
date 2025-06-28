using psdPH.Logic;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Utils.Setups
{
    public class CheckSetup:Setup
    {
        public CheckSetup(ReflectionConfig config):base(config)
        {
            var chb = new CheckBox();
            Control = chb;
            chb.IsChecked = (bool?)config.GetValue();
            _stack.Children.Add(chb);
            valueFunc = () => chb.IsChecked;
            isValidFunc = (obj) => obj is bool;
        }
    }
}
