using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Logic;
using System;

namespace psdPH.Views.WeekView
{
    [Obsolete]
    public class WeekRulesetStackHandler : RuleStackHandler
    {
        public WeekRulesetStackHandler(RuleSet ruleSet, Document doc) : base(ruleSet, doc) { }
        protected override RuleCommand RuleCommand => new WeekRuleCommand(RuleSet);
    }
    public class WeekDayRulesetStackHandler : RuleStackHandler
    {
        public WeekDayRulesetStackHandler(RuleSet ruleSet, Document doc) : base(ruleSet, doc) { }
        protected override RuleCommand RuleCommand => new WeekDayRuleCommand(RuleSet);
    }
}
