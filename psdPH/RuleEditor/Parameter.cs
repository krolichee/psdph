using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Serialization;
using static psdPH.Logic.Parameter;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public class ParameterConfig
    {
        object Obj;
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
        public Func<object, object> PullFunction = (o) => (o);
        public Func<object, object> PushFunction = (o) => (o);

        public static FieldFunctions EnumWrapperFunctions => new FieldFunctions()
        {
            PushFunction = (o => new EnumWrapper(o as Enum)),
            PullFunction = (o) => (o as EnumWrapper).Value
        };
    }
    
    public class Parameter
    {
        StackPanel _stack;
        Func<bool> accept;
        private ParameterConfig _config;
        public ParameterConfig Config => _config; 
        
        public static Parameter Choose(ParameterConfig config, object[] options, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            var result = new Parameter(config);
            var stack = result._stack;
            var cb = new ComboBox() { ItemsSource = options.Select(fieldFunctions.PushFunction) };
            cb.SelectedValue = 
            stack.Children.Add(cb);
            result.accept = () => { config.SetValue(fieldFunctions.PullFunction(cb.SelectedValue)); return true; };
            return result;
        }
        public static Parameter StringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var tb = new TextBox() {Width = 40};
            tb.Text = config.GetValue().ToString();
            stack.Children.Add(tb);
            result.accept = () => { config.SetValue(tb.Text); return true; };
            return result;
        }
        public static Parameter IntInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;
            var ntb = new NumericTextBox();
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
            chb.IsChecked = (bool)config.GetValue();
            stack.Children.Add(chb);
            result.accept = () => { config.SetValue(chb.IsChecked); return true; };
            return result;
        }
        public class EnumWrapper
        {
            public Enum Value;
            public EnumWrapper(Enum value){
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
            var options = enumValues.Select(e => new EnumWrapper(e)).ToArray();
            return Parameter.Choose(config, options, FieldFunctions.EnumWrapperFunctions);
        }
        Parameter(ParameterConfig config)
        {
            _stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
        }
        public Parameter() : this(null) { }
        public void Accept()
        {
            accept();
        }
        public StackPanel Stack => _stack;

    }
}
