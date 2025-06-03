using psdPH.Logic;
using psdPH.RuleEditor;
using System;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Views.WeekView
{
    [Obsolete]
    public class WeekRuleCommand : RuleCommand
    {
        public WeekRuleCommand(RuleSet ruleSet) : base(ruleSet) {
            RulesetDefinition = new WeekRulesetDefinition(ruleSet.Composition);
        }
    }
    public class WeekDayRuleCommand : RuleCommand
    {
        public WeekDayRuleCommand(RuleSet ruleSet) : base(ruleSet) {
            RulesetDefinition = new DayRulesetDefinition(ruleSet.Composition);
        }
        
    }
}
