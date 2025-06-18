using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Logic;
using System;
using psdPH.Logic.Parameters;

namespace psdPH.Views.WeekView
{
    [Obsolete]
    public class WeekRulesetStackHandler : RuleStackHandler
    {
        WeekListData WeekListData;
        public WeekRulesetStackHandler(WeekListData weekListData) : base(weekListData.WeekRulesets.DayRules)
        { WeekListData = weekListData; }
        protected override RuleCommand RuleCommand => new WeekRuleCommand(WeekListData);
    }

    public class WeekDayRulesetStackHandler : RuleStackHandler
    {
        WeekListData WeekListData;
        public WeekDayRulesetStackHandler(WeekListData weekListData) : base(weekListData.WeekRulesets.DayRules) 
        { WeekListData = weekListData; }
        protected override RuleCommand RuleCommand => new WeekDayRuleCommand(WeekListData);
    }

}
