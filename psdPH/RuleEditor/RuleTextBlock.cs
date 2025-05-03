using psdPH.Logic;
using System.Collections.Generic;
using System.Windows.Controls;
using Rule = psdPH.Logic.Rule;

namespace psdPH.RuleEditor
{
    class RuleTextBlock : TextBlock
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
        string[] getConditionParts(ConditionRule rule)
        {
            List<string> parts = new List<string>();
            Logic.Rules.Condition condition = rule.Condition;
            Parameter[] conditionParameters = condition.Parameters;
            parts.Add("Если");
            parts.Add(condition.ToString());
            parts.Add(getText(conditionParameters));
            parts.Add(", то");
            return parts.ToArray();
        }
        void textblockView(Rule rule)
        {
            Parameter[] ruleParameters = rule.Parameters;
            List<string> parts = new List<string>();
            if (rule is ConditionRule)
                parts.AddRange(getConditionParts((ConditionRule)rule));
            parts.Add(rule.ToString());
            parts.Add(getText(ruleParameters));
            Text = string.Join(" ", parts);
            TextWrapping = System.Windows.TextWrapping.Wrap;
        }
        public RuleTextBlock(Rule rule)
        {
            textblockView(rule);
        }
    }
}
