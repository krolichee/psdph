using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System.Linq;
using System.Windows;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.RuleEditor
{
    /// <summary>
    /// Логика взаимодействия для RuleControlWindow.xaml
    /// </summary>
    public partial class RuleEditorWindow : Window, IBatchRuleEditor
    {

        Rule[] _result = new Rule[0];
        RuleControl _rc;
        RulesetDefinition getValidatedRuleset(RulesetDefinition rulesetDef)
        {
            bool canBeSetUp(ISetupable s)
            {
                try { var _ = s.Setups; return true; } catch { return false; }
            }

            var rules = rulesetDef.Rules;
            var conditions = rulesetDef.Conditions;

            var valid_rules = rules.Where(canBeSetUp).ToArray();
            var valid_conditions = conditions.Where(canBeSetUp).ToArray();
            return new RulesetDefinition(valid_rules, valid_conditions);
        }
        bool anyRulesAndConditions(RulesetDefinition rulesetDef)
        {
            return rulesetDef.Conditions.Any() && rulesetDef.Rules.Any();
        }
        public RuleEditorWindow(RulesetDefinition rulesetDef)
        {
            this.CenterByTopmostOrScreen();
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;

            var valid_rulesetDef = getValidatedRuleset(rulesetDef);
            if (!anyRulesAndConditions(valid_rulesetDef))
            {
                MessageBox.Show("Нет подходящих правил или условий для данного подшаблона. Попробуйте изменить структуру");
                IsEnabled = false;
            }

            _rc = new RuleControl(valid_rulesetDef);

            _rc.Margin = new Thickness(10, 10, 10, 10);
            mainGrid.Children.Add(_rc);
        }

        public Rule[] GetResultBatch()
        {
            return  _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _result = _rc.GetResultBatch();
            Close();
        }
    }
}
