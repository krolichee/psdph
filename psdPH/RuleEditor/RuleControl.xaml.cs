using psdPH.Logic.Rules;
using System.Collections.Generic;
using System.Windows.Controls;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    /// <summary>
    /// Логика взаимодействия для RuleControl.xaml
    /// </summary>
    public partial class RuleControl : UserControl
    {
        ConditionRule _result;
        Condition _condition;
        List<Parameter> _parameters = new List<Parameter>();
        public RuleControl(Rule[] rules, Condition[] conditions)
        {
            InitializeComponent();
            conditionsComboBox.ItemsSource = conditions;
            ruleComboBox.ItemsSource = rules;
        }

        private void conditionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _condition = conditionsComboBox.SelectedItem as Condition;
            conditionParametersStack.Children.Clear();
            var parameters = _condition.Setups;
            _parameters.AddRange(parameters);
            foreach (var item in parameters)
            {
                conditionParametersStack.Children.Add(item.Stack);
            }
        }
        private void ruleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _result = ruleComboBox.SelectedItem as ConditionRule;
            ruleParametersStack.Children.Clear();
            var parameters = _result.Setups;
            _parameters.AddRange(parameters);
            foreach (var item in parameters)
            {
                ruleParametersStack.Children.Add(item.Stack);
            }

        }
        void acceptParameters()
        {
            foreach (var item in _parameters)
                item.Accept();
        }

        public ConditionRule GetResultRule()
        {
            acceptParameters();
            _result.Condition = _condition;
            return _result;
        }
    }
}
