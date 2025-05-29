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
        bool _applied = false;
        public bool Applied => _applied;
        StackPanel _stack;
        Parameter[] _parameters;
        Parameter[] Parameters
        {
            set
            {
                _parameters = value;
                _stack.Children.Clear();
                foreach (var p in _parameters)
                    _stack.Children.Add(p.Stack);
            }
            get => _parameters;
        }
        public ParametersInputWindow(Parameter[] parameters, string title = "")
        {
            
            InitializeComponent();
            this.CenterByTopmostOrScreen();


            Title = title;
            _stack = new StackPanel();
            Parameters = parameters;
            foreach (var parameter in parameters)
            {
                parameter.Stack.Orientation = Orientation.Vertical;
                parameter.Control.HorizontalAlignment = HorizontalAlignment.Left;
                parameter.Control.VerticalAlignment = VerticalAlignment.Top; 
                parameter.Stack.Margin = new Thickness(10, 10, 0, 10);

            }
            MainGrid.Children.Insert(0, _stack);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _applied = true;
            foreach (var par in _parameters)
                par.Accept();
            Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _stack.Children.Clear();
        }
    }
}
