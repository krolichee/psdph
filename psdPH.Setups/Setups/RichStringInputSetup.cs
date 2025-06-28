using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using psdPH.Utils.ReflectionParameter;
using psdPH.Setups;

namespace psdPH.Utils.Setups
{
    public class RichStringInputSetup: Setup
    {
        public RichStringInputSetup(ReflectionConfig config):base(config)
        {

            var rtb = new TextBox() {AcceptsReturn = true,  MinWidth = 70, MaxWidth = 200, MinHeight = 40, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };

           // rtb.TextChanged += RichTextBox_TextChanged;
            valueFunc = () => 
            rtb.Text.Replace("\r","");
            Control = rtb;
            rtb.Text = config.GetValue() as string;
            _stack.Children.Add(rtb);

        }
    }
}
