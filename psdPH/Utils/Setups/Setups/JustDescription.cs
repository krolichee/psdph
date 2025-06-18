using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Utils.Setups
{
    public class JustDescription:Setup
    {
        public static Setup JustDescrition(string desc)
        {
            var label = new Label() { Content = "" };
            var config = new SetupConfig(label, nameof(label.Content), desc);

            var result = new JustDescription(config);
            var stack = result._stack;
            result.Control = label;
            result.valueFunc = () => ""; ;
            return result;
        }
        protected JustDescription(SetupConfig config) : base(config) { }
    }
}
