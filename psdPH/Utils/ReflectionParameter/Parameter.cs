using psdPH.Utils.ReflectionParameter;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Logic
{
    public class Parameter
    {
        public delegate void AcceptedEvent();
        public event AcceptedEvent Accepted;
        public Parameter() { }
        public Control Control;
        FieldFunctions _fieldFunctions;
        StackPanel _stack;

        Func<object> valueFunc;

        private ParameterConfig _config;
        public ParameterConfig Config => _config;
        public void Accept()
        {
            _config.SetValue(valueFunc());
            Accepted?.Invoke();
        }
        public string ValueToString()
        {
            return _fieldFunctions.ConvertFunction(_config.GetValue()).ToString();
        }
        public static Parameter Choose(ParameterConfig config, object[] options, FieldFunctions fieldFunctions = null)
        {
            var result = new Parameter(config, fieldFunctions);
            fieldFunctions = result._fieldFunctions;
            var stack = result._stack;
            var cb = new ComboBox() { ItemsSource = options.Select(fieldFunctions.ConvertFunction) };
            result.valueFunc = () => fieldFunctions.RevertFunction(cb.SelectedValue);
            result.Control = cb;
            stack.Children.Add(cb);
            return result;
        }

        public static Parameter RichStringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;

            var rtb = new RichTextBox() { MinWidth = 70, MinHeight = 40 };

            rtb.TextChanged += RichTextBox_TextChanged;
            result.valueFunc = () => rtb.GetText();
            result.Control = rtb;
            rtb.SetText(config.GetValue() as string);;
            stack.Children.Add(rtb);
            return result;

            void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                foreach (Paragraph item in (sender as RichTextBox).Document.Blocks)
                    item.Margin = new Thickness(0, 0, 0, 0);
            }
            
        }
        public static Parameter AlignmentInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var aliControl = new AlignmentControl(config.GetValue() as Alignment);
            aliControl.Dimension = 30;
            result.Control = aliControl;
            stack.Children.Add(aliControl);
            result.valueFunc = () => aliControl.GetResultAlignment();
            return result;
        }
        public static Parameter StringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var tb = new TextBox() { Width = 40 };
            tb.Text = config.GetValue().ToString();
            result.Control = tb;
            stack.Children.Add(tb);
            result.valueFunc = () => tb.Text;
            return result;
        }
        public static Parameter IntInput(ParameterConfig config, int? min = null, int? max = null)
        {
            var result = new Parameter(config);
            var stack = result._stack;

            var ntb = new NumericTextBox((int)config.GetValue(),min,max);
            result.Control = ntb;
            stack.Children.Add(ntb);
            result.valueFunc = () => ntb.GetNumber();
            return result;
        }
        public static Parameter Check(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;

            var chb = new CheckBox();
            result.Control = chb;
            chb.IsChecked = (bool)config.GetValue();
            stack.Children.Add(chb);
            result.valueFunc = () => chb.IsChecked; ;
            return result;
        }
        public static Parameter EnumChoose(ParameterConfig config, Type @enum)
        {
            var enumValues = Enum.GetValues(@enum).Cast<Enum>();
            var options = enumValues.ToArray();
            return Parameter.Choose(config, options, FieldFunctions.EnumWrapperFunctions);
        }

        internal static Parameter Calendar(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;

            var calendar = new Calendar();
            result.Control = calendar;

            stack.Children.Add(calendar);
            result.valueFunc=()=>calendar.SelectedDate;

            return result;
        }

        Parameter(ParameterConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel();
            _stack.Orientation = Orientation.Horizontal;
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }

        public StackPanel Stack => _stack;

    }
}
