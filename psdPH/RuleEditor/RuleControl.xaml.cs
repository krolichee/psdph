using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.RuleEditor;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Condition = psdPH.Logic.Rules.Condition;
using Rule = psdPH.Logic.Ruleset.Rules.Rule;

namespace psdPH.Logic
{
    /// <summary>
    /// Логика взаимодействия для RuleControl.xaml
    /// </summary>
    public partial class RuleControl : UserControl,IBatchRuleEditor
    {
        ConditionRule _rule;
        Condition _condition;
        List<Setup> _conditionSetups = new List<Setup>();
        List<Setup> _ruleSetups = new List<Setup>();
        bool accepting=false;
        void initComboBoxes(Rule[] rules, Condition[] conditions)
        {
            conditionsComboBox.ItemsSource = conditions;
            ruleComboBox.ItemsSource = rules;
            subsribe(rules,conditions);
        }
        void subsribe(IEnumerable<ISetupable> rules, IEnumerable<ISetupable> conditions)
        {
            foreach (var item in conditions.Concat(rules))
                item.SetupsChanged += SetupsChanged;
        }
        void initComboBoxes(RulesetDefinition rulesetDef)
        {
            initComboBoxes(rulesetDef.Rules, rulesetDef.Conditions);
        }
        public RuleControl(RulesetDefinition rulesetDef) {
            InitializeComponent();
            initComboBoxes(rulesetDef);
        }
        void replaceIfSameType<T>(List<T> list,T replacement)
        {
            T inList;
            try
            {
                var type = replacement.GetType();
                inList = list.First(r => r.GetType() == type);
            }
            catch { return; }
            var index = list.IndexOf(inList);
            list.RemoveAt(index);
            list.Insert(index, replacement);
        }
        void SetupsChanged(object sender)
        {
            if (accepting)
                return;

            if (sender == _condition)
                RefreshConditionSetupsStack();
            else if (sender == _rule)
                RefreshRuleSetupsStack();
        }
        
        void RefreshConditionSetupsStack()
        {
            conditionParametersStack.Children.Clear();
            _conditionSetups = _condition.Setups.ToList();
            foreach (var param in _conditionSetups)
            {
                setupParameterApperiance(param);
                conditionParametersStack.Children.Add(param.Stack);
            }
        }
        void RefreshRuleSetupsStack()
        {
            ruleParametersStack.Children.Clear();
            _ruleSetups = _rule.Setups.ToList();
            foreach (var param in _ruleSetups)
            {
                setupParameterApperiance(param);
                ruleParametersStack.Children.Add(param.Stack);
            }
        }

        public RuleControl(ConditionRule rule_original, RulesetDefinition rulesetDef)
        {

            var rule = rule_original.Clone() as ConditionRule;
            var condition = rule.Condition;

            var conditions = rulesetDef.Conditions.ToList();
            var rules = rulesetDef.Rules.ToList();

            replaceIfSameType(conditions, condition);
            replaceIfSameType(rules, rule);

            subsribe(rules, conditions);

            InitializeComponent();
            initComboBoxes(rules.ToArray(),conditions.ToArray());
            conditionsComboBox.SelectedItem = condition;
            ruleComboBox.SelectedItem = rule;


        }

        void setupParameterApperiance(Setup param)
        {
            param.Stack.Orientation = Orientation.Horizontal;
            param.Control.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            param.Control.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            param.Stack.Margin = new System.Windows.Thickness(0, 0, 0, 10);
        }

        private void conditionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _condition = conditionsComboBox.SelectedItem as Condition;
            RefreshConditionSetupsStack();
        }
        private void ruleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rule = ruleComboBox.SelectedItem as ConditionRule;
            RefreshRuleSetupsStack();
        }
        void acceptParameters()
        {
            accepting = true;
            foreach (var item in _ruleSetups.Concat(_conditionSetups))
                item.Accept();
        }
        bool IsRuleSetUp()
        {
            bool? ruleSetUp = _rule?.IsSetUp();
            bool? conditionSetUp = _condition?.IsSetUp();
            return ruleSetUp  == true &&
                 conditionSetUp == true;
        }
        ConditionRule GetResultRule()
        {
            acceptParameters();
            if (!IsRuleSetUp())
                return null;

            _rule.Condition = _condition;
            return _rule;
        }

        public bool? ShowDialog()
        {
            throw new System.NotImplementedException();
        }

        public Rule[] GetResultBatch()
        {
            var rule = GetResultRule();
            return rule != null ? new[] { rule } : new Rule[0];
        }
    }
}
