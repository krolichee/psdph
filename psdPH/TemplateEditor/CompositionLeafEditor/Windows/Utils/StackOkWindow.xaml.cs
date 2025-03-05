using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils
{
    /// <summary>
    /// Логика взаимодействия для StackOkWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        bool applied = false;
        public bool Applied => applied;
        StackPanel stack;
        Parameter[] parameters;
        Parameter[] Parameters { set
            {
                parameters = value;
                foreach (var p in parameters)
                    stack.Children.Add(p.Stack);
            }
            get => parameters;
        }
        public ParametersWindow(Parameter[] parameters,string title="")
        {
            InitializeComponent();
            Title = title;
            stack = new StackPanel();
            Parameters = parameters;
            foreach (var item in parameters)
            {
                item.Stack.Orientation = Orientation.Vertical;
                item.Control.HorizontalAlignment = HorizontalAlignment.Left;
                item.Stack.Margin = new Thickness(0,0,0,10);
            }
            MainGrid.Children.Insert(0, stack);
        }
        static int GetGlobalWeekNumber(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1); // Начало отсчета
            TimeSpan diff = date - epoch;
            return (int)(diff.TotalDays / 7) + 1; // Номер недели с 1 января 1970 года
        }
        static int GetWeekOfYear(DateTime date)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            System.Globalization.Calendar calendar = culture.Calendar;
            return calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
        }
        private void Button_Click(object sender, RoutedEventArgs e)

        {
            foreach (var par in parameters)
                par.Accept();
            applied = true;
            Close();
        }
    }
}
