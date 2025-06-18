using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.Utils;
using psdPH.Utils.Setups;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils
{
    /// <summary>
    /// Логика взаимодействия для StackOkWindow.xaml
    /// </summary>
    public partial class SetupsInputWindow : Window
    {
        bool _applied = false;
        public bool Applied => _applied;
        StackPanel _stack;
        Setup[] _setups;
        Setup[] Setups
        {
            set
            {
                _setups = value;
                _stack.Children.Clear();
                foreach (var p in _setups)
                    _stack.Children.Add(p.Stack);
            }
            get => _setups;
        }
        void formatParameterUI(Setup setup)
        {
            setup.Stack.Orientation = Orientation.Vertical;
            setup.Control.HorizontalAlignment = HorizontalAlignment.Left;
            setup.Control.VerticalAlignment = VerticalAlignment.Top;
            setup.Stack.Margin = new Thickness(3, 0, 4, 0);
        }
        public SetupsInputWindow(Setup setup, string title = "") : this(new Setup[] { setup }, title) { }
        public SetupsInputWindow(Setup[] setups, string title = "")
        {
            InitializeComponent();
            this.CenterByTopmostOrScreen();

            Title = title;
            _stack = new StackPanel();
            Setups = setups;
            foreach (var setup in setups)
                formatParameterUI(setup);
            MainGrid.Children.Insert(0, _stack);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _applied = true;
            foreach (var setup in _setups)
                setup.Accept();
            Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _stack.Children.Clear();
        }
    }
}
