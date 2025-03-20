using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Serialization;
using static psdPH.Logic.Parameter;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public class ParameterConfig
    {
        public object Obj;
        public string FieldName;
        public string Desc;
        public Type GetTypeOfObj()
        {
            return Obj.GetType();
        }
        public void SetValue(object value)
        {
            Type objType = GetTypeOfObj();
            FieldInfo fieldInfo = objType.GetField(FieldName);
            PropertyInfo propertyInfo = objType.GetProperty(FieldName);

            if (fieldInfo != null)
                fieldInfo.SetValue(Obj, value);
            else if (propertyInfo != null)
                propertyInfo.SetValue(Obj, value);
            else
                throw new ArgumentException($"Поле или свойство с именем '{FieldName}' не найдено в объекте типа '{objType.Name}'.");
        }

        public object GetValue()
        {
            Type objType = GetTypeOfObj();
            FieldInfo fieldInfo = objType.GetField(FieldName);
            PropertyInfo propertyInfo = objType.GetProperty(FieldName);

            if (fieldInfo != null)
                return fieldInfo.GetValue(Obj);
            else if (propertyInfo != null)
                return propertyInfo.GetValue(Obj);
            else
                throw new ArgumentException($"Поле или свойство с именем '{FieldName}' не найдено в объекте типа '{objType.Name}'.");
        }
        public ParameterConfig(object obj, string fieldname, string desc)
        {
            this.Obj = obj;
            this.FieldName = fieldname;
            this.Desc = desc;
        }
    }
    public class FieldFunctions
    {
        public Func<object, object> RevertFunction = (o) => (o);
        public Func<object, object> ConvertFunction = (o) => (o);

        public static FieldFunctions EnumWrapperFunctions => new FieldFunctions()
        {
            ConvertFunction = (o => new EnumWrapper(o as Enum)),
            RevertFunction = (o) => (o as EnumWrapper).Value
        };
    }

    public class Parameter
    {
        public Control Control;
        FieldFunctions _fieldFunctions;
        StackPanel _stack;
        Func<bool> accept;

        private ParameterConfig _config;
        public ParameterConfig Config => _config;
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
            result.accept = () => { config.SetValue(fieldFunctions.RevertFunction(cb.SelectedValue)); return true; };

            result.Control = cb;
            stack.Children.Add(cb);
            return result;
        }
        static string getRtbText(RichTextBox rtb, string lineSep = "\n")
        {
            string result = "";
            foreach (Paragraph item in (rtb).Document.Blocks)
                foreach (Run item1 in item.Inlines)
                {
                    result += item1.Text;
                    result += lineSep;
                }
            return result;
        }
        public static Parameter RichStringInput(ParameterConfig config)
        {
            void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                foreach (Paragraph item in (sender as RichTextBox).Document.Blocks)
                    item.Margin = new Thickness(0, 0, 0, 0);
            }
            
            var result = new Parameter(config);
            var stack = result._stack;
            var rtb = new RichTextBox() { Width = 70,Height = 30 };
            rtb.TextChanged += RichTextBox_TextChanged;
            result.accept = () =>
            {
                config.SetValue(getRtbText(rtb)); return true;
            };

            result.Control = rtb;
            stack.Children.Add(rtb);
            return result;
        }
        public static Parameter StringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var tb = new TextBox() { Width = 40 };
            result.Control = tb;
            tb.Text = config.GetValue().ToString();
            stack.Children.Add(tb);
            result.accept = () =>
            {
                config.SetValue(tb.Text); return true;
            };
            return result;
        }
        public static Parameter IntInput(ParameterConfig config, int? min = null, int? max = null)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var ntb = new NumericTextBox();
            result.Control = ntb;
            ntb.Text = config.GetValue().ToString();
            stack.Children.Add(ntb);
            result.accept = () => { config.SetValue(ntb.GetNumber()); return true; };

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
            result.accept = () => { config.SetValue(chb.IsChecked); return true; };
            return result;
        }
        public class EnumWrapper
        {
            public Enum Value;
            public EnumWrapper(Enum value)
            {
                Value = value;
            }
            public override string ToString()
            {
                return Value.GetDescription();
            }
        }

        public static Parameter EnumChoose(ParameterConfig config, Type @enum)
        {

            var enumValues = Enum.GetValues(@enum).Cast<Enum>();
            var options = enumValues.ToArray();
            return Parameter.Choose(config, options, FieldFunctions.EnumWrapperFunctions);
        }
        Parameter(ParameterConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            else
                ;
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }
        public Parameter() : this(null) { }
        public void Accept()
        {
            accept();
        }

        public StackPanel Stack => _stack;

    }
}
