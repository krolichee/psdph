using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public RuleControl(Composition composition)
        {
            InitializeComponent();
            var conditions = new Condition[]
            {
                new MaxRowCountCondition(composition),
                new MaxRowLenCondition(composition)
            };
            comboBox.DisplayMemberPath = nameof(Condition.UIName);
            comboBox.ItemsSource = conditions;
            var rules = new Rule[]
            {
                new TextFontSizeRule(composition),
                new TextAnchorRule(composition),
            };
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _condition = comboBox.SelectedItem as Condition;
            conditionParametersStack.Children.Clear();
            foreach (var item in _condition.Parameters)
            {
                conditionParametersStack.Children.Add(item.Stack);
            }

        }
    }
}
