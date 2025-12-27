using System.Windows.Controls;
using System.Windows;
using psdPH.Setups;
using psdPH.Reflection;

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
