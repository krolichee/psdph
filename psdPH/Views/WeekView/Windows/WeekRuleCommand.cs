using psdPH.Logic;
using System;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Views.WeekView
{
    [Obsolete]
    public class WeekRuleCommand : RuleCommand
    {
        public WeekRuleCommand(RuleSet ruleSet) : base(ruleSet) { }
        public override Condition[] Conditions => WeekRules.WeekConditions(RuleSet.Composition);

        public override Rule[] Rules => WeekRules.Rules(RuleSet.Composition);
    }
    public class WeekDayRuleCommand : RuleCommand
    {
        public WeekDayRuleCommand(RuleSet ruleSet) : base(ruleSet) { }

        public override Condition[] Conditions => WeekRules.DayConditions(RuleSet.Composition);

        public override Rule[] Rules => WeekRules.Rules(RuleSet.Composition);
    }
}
