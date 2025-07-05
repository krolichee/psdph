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
    public class JustDescriptionSetup:Setup
    {
        public static Setup JustDescription(string desc)
        {
            var label = new Label() { Content = "" };
            var config = new ReflectionConfig(label, nameof(label.Content), desc);

            var result = new JustDescriptionSetup(config);
            var stack = result._stack;
            result.Control = label;
            result.valueFunc = () => ""; ;
            return result;
        }
        protected JustDescriptionSetup(ReflectionConfig config) : base(config) { }
    }
}
