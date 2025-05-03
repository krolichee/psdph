using psdPH.Logic;
using System.Windows;

namespace psdPH.RuleEditor
{
    /// <summary>
    /// Логика взаимодействия для RuleControlWindow.xaml
    /// </summary>
    public partial class RuleControlWindow : Window, IRuleEditor
    {
        ConditionRule _result;
        RuleControl _rc;
        public RuleControlWindow(Composition composition)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            _rc = new RuleControl(composition);
            mainGrid.Children.Add(_rc);
        }

        public ConditionRule GetResultRule()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _result = _rc.GetResultRule();
            Close();
        }
    }
}
