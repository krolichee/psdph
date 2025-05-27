using psdPH.Logic;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.RuleEditor
{
    /// <summary>
    /// Логика взаимодействия для RuleViewControl.xaml
    /// </summary>
    public partial class RuleViewControl : UserControl
    {
        StackPanel _sp;
        void addToSP(Parameter[] parameters)
        {
            var sp_ch = _sp.Children;
            foreach (var parameter in parameters)
            {                
                var config = parameter.Config;
                sp_ch.Add(new Label() { Content = config.Desc });
                sp_ch.Add(new Label() { Content = config.GetValue().ToString() });
            }
        }
        void labelViev(ConditionRule rule)
        {
            _sp = new StackPanel() { Orientation = Orientation.Horizontal };
            Content = _sp;
            Parameter[] conditionParameters = rule.Condition.Setups;
            Parameter[] ruleParameters = rule.Setups;
            var sp_ch = _sp.Children;
            sp_ch.Add(new Label() { Content = "Если" });
            sp_ch.Add(new Label() { Content = rule.Condition.ToString() });
            addToSP(conditionParameters);
            sp_ch.Add(new Label() { Content = ", то" });
            sp_ch.Add(new Label() { Content = rule.ToString() });
            addToSP(ruleParameters);
        }
        string getText(Parameter[] parameters)
        {
            List<string> parts = new List<string>();
            foreach (var parameter in parameters)
            {
                var config = parameter.Config;
                parts.Add(config.Desc);
                parts.Add(UIName.ToString(config.GetValue()));
            }
            return string.Join(" ", parts);

        }
        void textblockView(ConditionRule rule)
        {
            Parameter[] conditionParameters = rule.Condition.Setups;
            Parameter[] ruleParameters = rule.Setups;

            var tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            Content = tb;
            List<string> parts = new List<string>();
            parts.Add("Если");
            parts.Add(rule.Condition.ToString());
            parts.Add(getText(conditionParameters));
            parts.Add(", то");
            parts.Add(rule.ToString());
            parts.Add(getText(ruleParameters));
            tb.Text = string.Join(" ", parts);
        }
        public RuleViewControl(ConditionRule rule)
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            textblockView(rule);

        }
    }
}
