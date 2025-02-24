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
    
    public class Parameter
    {
        StackPanel _stack;
        Func<bool> accept;
        private ParameterConfig _config;

        public ParameterConfig Config => _config; 

        public static Parameter Choose(ParameterConfig config, object[] options)
        {
            var result = new Parameter(config);
            var stack = result._stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            stack.Children.Add(new Label() { Content = config.Desc });
            var cb = new ComboBox() { ItemsSource = options };
            stack.Children.Add(cb);
            result.accept = () => { config.SetValue(cb.SelectedValue); return true; };
            return result;
        }
        public static Parameter StringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            stack.Children.Add(new Label() { Content = config.Desc });
            var tb = new TextBox() {};
            stack.Children.Add(tb);
            result.accept = () => { config.SetValue(tb.Text); return true; };
            return result;
        }
        public static Parameter IntInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            stack.Children.Add(new Label() { Content = config.Desc });
            var ntb = new NumericTextBox();
            stack.Children.Add(ntb);
            result.accept = () => { config.SetValue(ntb.GetNumber()); return true; };
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
            var enumValues = Enum.GetValues(@enum).Cast<Enum>().ToArray();
            var options = new List<EnumWrapper>();
            foreach (var item in enumValues)
            {
                options.Add(new EnumWrapper(item));
            }
            var result = new Parameter(config);
            var stack = result._stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            stack.Children.Add(new Label() { Content = config.Desc });
            var cb = new ComboBox() { ItemsSource = options };
            stack.Children.Add(cb);
            result.accept = () => { config.SetValue((cb.SelectedValue as EnumWrapper).Value); return true; };
            return result;
        }
        Parameter(ParameterConfig config)
        {
            _config = config;
        }
        public void Accept()
        {
            accept();
        }
        public StackPanel Stack => _stack;

    }
}
