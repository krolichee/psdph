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

        void init()
        {
            this.CenterByTopmostOrScreen();
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }
        RulesetDefinition validateRulesetDef(RulesetDefinition rulesetDef)
        {
            var valid_rulesetDef = getValidatedRuleset(rulesetDef);
            if (!anyRulesAndConditions(valid_rulesetDef))
            {
                MessageBox.Show("Нет подходящих правил или условий для данного подшаблона. Попробуйте изменить структуру");
                IsEnabled = false;
            }
            return valid_rulesetDef;
        }
        public RuleEditorWindow(RulesetDefinition rulesetDef)
        {
            init();
            var valid_rulesetDef = validateRulesetDef(rulesetDef);
            _rc = new RuleControl(valid_rulesetDef);

            _rc.Margin = new Thickness(10, 10, 10, 10);
            mainGrid.Children.Add(_rc);
        }
        public RuleEditorWindow(ConditionRule rule,RulesetDefinition rulesetDef)
        {
            _result = new Rule[] { rule };

            init();
            var valid_rulesetDef = validateRulesetDef(rulesetDef);
            _rc = new RuleControl(rule.Clone() as ConditionRule,valid_rulesetDef);

            _rc.Margin = new Thickness(10, 10, 10, 10);
            mainGrid.Children.Add(_rc);
        }
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

        public Rule[] GetResultBatch()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            Rule[] ruleBatch;
            try
            {
                ruleBatch = _rc.GetResultBatch();
                success = ruleBatch.Length != 0;
                if (success)
                    _result = ruleBatch;
            }
            catch { }
            
            if (!success)
                MessageBox.Show("Правило является некорректным. Возможно, были пропущены какие-либо параметры");
            Close();
        }
    }
}
