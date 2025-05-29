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

        void setupParameterApperiance(Parameter param)
        {
            param.Stack.Orientation = Orientation.Horizontal;
            param.Control.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            param.Control.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            param.Stack.Margin = new System.Windows.Thickness(0, 0, 0, 10);
        }

        private void conditionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _condition = conditionsComboBox.SelectedItem as Condition;
            conditionParametersStack.Children.Clear();
            var parameters = _condition.Setups;
            _parameters.AddRange(parameters);
            foreach (var param in parameters)
            {
                setupParameterApperiance(param);
                conditionParametersStack.Children.Add(param.Stack);
            }
        }
        private void ruleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _result = ruleComboBox.SelectedItem as ConditionRule;
            ruleParametersStack.Children.Clear();
            var parameters = _result.Setups;
            _parameters.AddRange(parameters);
            foreach (var param in parameters)
            {
                setupParameterApperiance(param);
                ruleParametersStack.Children.Add(param.Stack);
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
