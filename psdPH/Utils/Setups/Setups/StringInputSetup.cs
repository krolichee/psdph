using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace psdPH.Utils.Setups
{
    public class StringInputSetup: Setup
    {
        public StringInputSetup(SetupConfig config):base(config)
        {
            var tb = new TextBox() { MinWidth = 40, TextWrapping = TextWrapping.Wrap, MaxWidth = 150 };
            tb.Text = config.GetValue() as string;
            Control = tb;
            _stack.Children.Add(tb);
            valueFunc = () => tb.Text;
            isValidValue = (obj) => obj is string;
        }
    }
}
