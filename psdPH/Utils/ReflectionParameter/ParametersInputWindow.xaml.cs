using psdPH.Logic;
using psdPH.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils
{
    /// <summary>
    /// Логика взаимодействия для StackOkWindow.xaml
    /// </summary>
    public partial class ParametersInputWindow : Window
    {
        bool applied = false;
        public bool Applied => applied;
        StackPanel stack;
        Parameter[] parameters;
        Parameter[] Parameters
        {
            set
            {
                parameters = value;
                foreach (var p in parameters)
                    stack.Children.Add(p.Stack);
            }
            get => parameters;
        }
        public ParametersInputWindow(Parameter[] parameters, string title = "")
        {
            Owner = TopmostWindow.Get();
            InitializeComponent();
            Title = title;
            stack = new StackPanel();
            Parameters = parameters;
            foreach (var parameter in parameters)
            {
                parameter.Stack.Orientation = Orientation.Vertical;
                parameter.Control.HorizontalAlignment = HorizontalAlignment.Left;
                parameter.Stack.Margin = new Thickness(0, 0, 0, 10);
            }
            MainGrid.Children.Insert(0, stack);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            applied = true;
            foreach (var par in parameters)
                par.Accept();
            Close();
        }
    }
}
