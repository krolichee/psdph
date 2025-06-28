using psdPH.Logic;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourNamespace;

namespace psdPH.Utils.Setups
{
    public class MultiChooseSetup: Setup
    {
        public MultiChooseSetup(ReflectionConfig config, object[] options):base(config)
        {
            var picker = new MultiPicker(options);
            Control = picker;
            _stack.Children.Add(picker);
            valueFunc = () => picker.GetSelectedItems();
        }
    }
}
