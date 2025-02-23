using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static psdPH.Logic.Parameter;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public class ParameterConfig
    {
        object Rule;
        public string FieldName;
        public string Desc;
        public Type GetTypeOfRule()
        {
            return Rule.GetType();
        }
        public void SetValue(object value) {
            FieldInfo fieldInfo = GetTypeOfRule().GetField(FieldName);

            // Устанавливаем значение поля
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(Rule, value); // Устанавливаем значение 42
            }
        }
        public static ParameterConfig CreateConfig<T>(Rule rule, Expression<Func<T>> field, string desc)
        {
            return new ParameterConfig(rule, GetFieldName(field), desc);
        }
        public ParameterConfig(object rule, string fieldname, string desc)
        {
            this.Rule = rule;
            this.FieldName = fieldname;
            this.Desc = desc;
        }
        public static string GetFieldName<T>(Expression<Func<T>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
    
    public class Parameter
    {
        
        enum ParameterType
        {

            StringInput,
            StringChoose
        }
        StackPanel _stack;
        Func<bool> accept;

        public static Parameter Choose(ParameterConfig config, object[] options)
        {
            var result = new Parameter();
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
            var result = new Parameter();
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
            var result = new Parameter();
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
            var result = new Parameter();
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
        Parameter()
        {

        }
        public StackPanel Stack => _stack;

    }
}
