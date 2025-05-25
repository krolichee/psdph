using psdPH.Utils;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;

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
        public Func<object, object> ConvertFunction = (o) => (o);
        public Func<object, object> RevertFunction = (o) => (o);

        public static FieldFunctions EnumWrapperFunctions => new FieldFunctions()
        {
            ConvertFunction = (o => new EnumWrapper(o as Enum)),
            RevertFunction = (o) => (o as EnumWrapper).Value
        };
    }
    
    public class Parameter
    {
        public Parameter() { }
        public Control Control;
        FieldFunctions _fieldFunctions;
        StackPanel _stack;

        Func<object> valueFunc;

        private ParameterConfig _config;
        public ParameterConfig Config => _config;
        void accept()
        {
            _config.SetValue(valueFunc());
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
        public static FlowDocument ConvertStringToFlowDocument(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new FlowDocument();

            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                paragraph.Inlines.Add(new Run(lines[i]));
                if (i < lines.Length - 1)
                    paragraph.Inlines.Add(new LineBreak());
            }
            flowDoc.Blocks.Add(paragraph);
            return flowDoc;
        }
        static string getRtbText(RichTextBox _rtb, string lineSep = "\n")
        {
            string _result = "";
            foreach (Paragraph item in (_rtb).Document.Blocks)
                foreach (var item1 in item.Inlines)
                    if (item1 is Run)
                    {
                        var run = (Run)item1;
                        if (_result != "")
                            _result += lineSep;
                        _result += run.Text;

                    }
            return _result;
        }

        public static Parameter RichStringInput(ParameterConfig config)
        {
            var result = new Parameter(config);
            var stack = result._stack;

            var rtb = new RichTextBox() { MinWidth = 70, MinHeight = 40 };

            rtb.TextChanged += RichTextBox_TextChanged;
            result.valueFunc = () => getRtbText(rtb, "\r");
            result.Control = rtb;
            rtb.Document = ConvertStringToFlowDocument(config.GetValue().ToString());
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
            //var aliControl = new AligmentControl();
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

            var ntb = new NumericTextBox();
            result.Control = ntb;
            ntb.Text = config.GetValue().ToString();
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
        Parameter(ParameterConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }
        public void Accept()
        {
            accept();
        }

        public StackPanel Stack => _stack;

    }
}
