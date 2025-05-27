using psdPH.Logic;
using psdPH.Logic.Rules;
using System.Windows;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.RuleEditor
{
    /// <summary>
    /// Логика взаимодействия для RuleControlWindow.xaml
    /// </summary>
    public partial class RuleControlWindow : Window, IRuleEditor
    {
        ConditionRule _result;
        RuleControl _rc;
        public RuleControlWindow(Rule[] rules, Condition[] conditions)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            _rc = new RuleControl(rules, conditions);
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
