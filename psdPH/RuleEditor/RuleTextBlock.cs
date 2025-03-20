using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.RuleEditor
{
    class RuleTextBlock:TextBlock
    {
        string getText(Parameter[] parameters)
        {
            List<string> parts = new List<string>();
            foreach (var parameter in parameters)
            {
                var config = parameter.Config;
                parts.Add(config.Desc);
                parts.Add(parameter.ValueToString());
            }
            return string.Join(" ", parts);

        }
        void textblockView(ConditionRule rule)
        {
            Parameter[] conditionParameters = rule.Condition.Parameters;
            Parameter[] ruleParameters = rule.Parameters;
            List<string> parts = new List<string>();
            parts.Add("Если");
            parts.Add(rule.Condition.ToString());
            parts.Add(getText(conditionParameters));
            parts.Add(", то");
            parts.Add(rule.ToString());
            parts.Add(getText(ruleParameters));
            Text = string.Join(" ", parts);
            TextWrapping = System.Windows.TextWrapping.Wrap;
        }
        public RuleTextBlock(ConditionRule rule)
        {
            textblockView(rule);
        }
    }
}
