using psdPH.Logic;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Label = System.Windows.Controls.Label;

namespace psdPH.Utils.Setups
{
    public class JustSeparator:Setup
    {
        public JustSeparator() : base(
            new ReflectionConfig(new Label() { Content = "" }, nameof(Label.Content), "")) {
            var separator = new Separator() { };
            Control = separator;
            valueFunc = () => "";
        }
    }
}
