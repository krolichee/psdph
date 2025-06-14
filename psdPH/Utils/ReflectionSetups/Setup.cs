using psdPH.Utils.ReflectionParameter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;
using YourNamespace;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Logic
{
    public class Setup
    {
        public delegate void AcceptedEvent();
        public event AcceptedEvent Accepted;
        private delegate void ChangedEvent();
        private event ChangedEvent Changed;
        public Setup() { }
        public FrameworkElement Control;
        FieldFunctions _fieldFunctions;
        StackPanel _stack;

        Func<object> valueFunc;

        private SetupConfig _config;
        public SetupConfig Config => _config;
        public void Accept()
        {
            _config.SetValue(valueFunc());
            Accepted?.Invoke();
        }
        public string ValueToString()
        {
            return _fieldFunctions.ConvertFunction(_config.GetValue()).ToString();
        }
        public static Setup Choose(SetupConfig config, object[] options, FieldFunctions fieldFunctions = null)
        {
            if (options.Length == 0)
                throw new ArgumentException();
            var result = new Setup(config, fieldFunctions);
            fieldFunctions = result._fieldFunctions;
            var stack = result._stack;

            var index = options.ToList().IndexOf(config.GetValue());

            var cb = new ComboBox() { ItemsSource = options.Select(fieldFunctions.ConvertFunction) };
            //var value = fieldFunctions.ConvertFunction(config.GetValue());

            cb.SelectedIndex = index;

            cb.SelectionChanged += (_,__)=> 
                result.Changed?.Invoke();

            result.valueFunc =  () => 
            fieldFunctions.RevertFunction(cb.SelectedValue);
            result.Control = cb;
            stack.Children.Add(cb);
            return result;
        }

        public static Setup RichStringInput(SetupConfig config)
        {
            var result = new Setup(config);
            var stack = result._stack;

            var rtb = new RichTextBox() {MinWidth = 70, MaxWidth = 200, MinHeight = 40,HorizontalAlignment = HorizontalAlignment.Left,VerticalAlignment=VerticalAlignment.Top};

            rtb.TextChanged += RichTextBox_TextChanged;
            result.valueFunc = () => rtb.GetText();
            result.Control = rtb;
            rtb.SetText(config.GetValue() as string);
            stack.Children.Add(rtb);
            return result;

            void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                foreach (Paragraph item in (sender as RichTextBox).Document.Blocks)
                    item.Margin = new Thickness(0, 0, 0, 0);
            }

        }
        public static Setup AlignmentInput(SetupConfig config)
        {
            var result = new Setup(config);
            var stack = result._stack;
            var aliControl = new AlignmentControl(config.GetValue() as Alignment);
            aliControl.Dimension = 30;
            result.Control = aliControl;
            stack.Children.Add(aliControl);
            result.valueFunc = () => aliControl.GetResultAlignment();
            return result;
        }
        public static Setup StringInput(SetupConfig config)
        {
            var result = new Setup(config);
            var stack = result._stack;
            var tb = new TextBox() { MinWidth = 40 };
            tb.Text = config.GetValue() as string;
            result.Control = tb;
            stack.Children.Add(tb);
            result.valueFunc = () => tb.Text;
            return result;
        }
        public static Setup IntInput(SetupConfig config, int? min = null, int? max = null)
        {
            var result = new Setup(config);
            var stack = result._stack;

            var ntb = new NumericTextBox((int)config.GetValue(), min, max);
            result.Control = ntb;
            stack.Children.Add(ntb);
            result.valueFunc = () => ntb.GetNumber();
            return result;
        }
        public static Setup Check(SetupConfig config)
        {
            var result = new Setup(config);
            var stack = result._stack;

            var chb = new CheckBox();
            result.Control = chb;
            chb.IsChecked = (bool?)config.GetValue();
            stack.Children.Add(chb);
            result.valueFunc = () => chb.IsChecked; ;
            return result;
        }
        public static Setup EnumChoose(SetupConfig config, Type @enum)
        {
            var enumValues = Enum.GetValues(@enum).Cast<Enum>();
            var options = enumValues.ToArray();
            return Setup.Choose(config, options, FieldFunctions.EnumWrapperFunctions);
        }
        public static Setup JustDescrition(string desc)
        {
            var label = new Label() { Content = "" };
            var config = new SetupConfig(label,nameof(label.Content),desc);

            var result = new Setup(config);
            var stack = result._stack;
            result.Control = label;
            result.valueFunc = () => ""; ;
            return result;
        }
        internal static Setup Date(SetupConfig config)
        {
            var result = new Setup(config);
            var stack = result._stack;

            var calendar = new DatePicker();
            var date = config.GetValue() as DateTime?;
            if (date!=null)
                calendar.SelectedDate = date;
            result.Control = calendar;

            stack.Children.Add(calendar);
            result.valueFunc = () => calendar.SelectedDate;

            return result;
        }

        internal static Setup MultiChoose(SetupConfig config, object[] options)
        {
            var result = new Setup(config);
            var stack = result._stack;

            var picker = new MultiPicker(options);
            result.Control = picker;

            stack.Children.Add(picker);
            result.valueFunc = () => picker.GetSelectedItems();

            return result;
        }

        internal static Setup JustSeparator()
        {
            var label = new Label() { Content = "" };
            var separator = new Separator() { };
            var config = new SetupConfig(label, nameof(label.Content), "");

            var result = new Setup(config);
            var stack = result._stack;
            result.Control = separator;
            result.valueFunc = () => ""; ;
            return result;
        }

        internal static Setup ComboString(SetupConfig config, ObservableCollection<string> strings)
        {

            var result = new Setup(config);
            var stack = result._stack;

            
            var cbStack = new StackPanel() { Orientation = Orientation.Horizontal };

            var cb = new ComboBox() { ItemsSource = strings, IsEditable = true, MinWidth = 70 };

            cb.SelectionChanged += (_, __) => result.Changed?.Invoke();


            void addExecute(object _) {
                var str = cb.Text;
                strings.Add(str);
            }
            void deleteExecute(object _) => strings.Remove(cb.Text);
            bool isInSelection(object _) => strings.Contains(cb.Text);
            bool isNotInSelection(object _) => !isInSelection(cb.Text);



            var addButton = new Button() { Content = "+" };
            addButton.Command = new RelayCommand(addExecute, isNotInSelection);
            

            var deleteButton = new Button() { Content = "-" };
            deleteButton.Command = new RelayCommand(deleteExecute,isInSelection);
            void refreshButtonsParametera(object sender, TextCompositionEventArgs e)
            {
                addButton.CommandParameter = e.Text;
                addButton.CommandParameter = e.Text;
            }
            cb.ItemsSource = strings;

            cb.PreviewTextInput += refreshButtonsParametera;

            //addButton.ToolTip = "Добавить в список";
            //ToolTipService.SetInitialShowDelay(addButton, 100);
            //ToolTipService.SetBetweenShowDelay(addButton, 500);
            //ToolTipService.SetShowDuration(addButton, 2000);

            cbStack.Children.Add(cb);
            cbStack.Children.Add(addButton);
            cbStack.Children.Add(deleteButton);

            cb.Text = config.GetValue() as string; ;
            result.valueFunc = () => cb.Text;

            stack.Children.Add(cbStack);
            result.Control = cb;
            return result;
        }

        

        Setup(SetupConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel();
            _stack.Orientation = Orientation.Horizontal;
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            if (config.AutoAccept)
                Changed += () => Accept();
        }

        public StackPanel Stack => _stack;

    }
}
