using psdPH.Logic;
using psdPH.RuleEditor;
using System;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Views.WeekView
{
    [Obsolete]
    public class WeekRuleCommand : RuleCommand
    {
        public WeekRuleCommand(WeekListData weekListData) : base(weekListData.WeekRulesets.DayRules)
        {
            RulesetDefinition = new WeekRulesetDefinition(weekListData);
        }
    }
    public class WeekDayRuleCommand : RuleCommand
    {
        public WeekDayRuleCommand(WeekListData weekListData) : base(weekListData.WeekRulesets.DayRules) {
            RulesetDefinition = new WeekDayRulesetDefinition(weekListData);
        }
        
    }
}
