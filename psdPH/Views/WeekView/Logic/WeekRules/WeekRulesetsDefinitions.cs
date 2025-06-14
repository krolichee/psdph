using psdPH.TemplateEditor;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using Condition = psdPH.Logic.Rules.Condition;
using System;
using psdPH.RuleEditor;
using System.Linq;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Logic.Ruleset.Rules.ParameterSetRules;
using psdPH.Logic.Parameters;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    static class WeekRulesetsDefinitions
    {
        public static Rule[] Rules(ParameterSet parameterSet) =>
                new Rule[]
                {
                    new FlagRule(parameterSet),
                    new SetStringValueRule(parameterSet)
                };
        [Obsolete]
        public static Condition[] WeekConditions() => new Condition[]
        {
            new WeekCondition()
        };

        public static Condition[] DayConditions() => new Condition[]
        {
            new EveryNDayCondition(),
            new DayOfWeekCondition()
        };
    };
    [Obsolete]
    public class WeekRulesetDefinition: RulesetDefinition
    {
        public WeekRulesetDefinition(WeekListData weekListData) : base(
            WeekRulesetsDefinitions.Rules(weekListData.MainBlob.ParameterSet),
            WeekRulesetsDefinitions.WeekConditions()
            )
        { }
    }
    public class WeekDayRulesetDefinition: RulesetDefinition
    {
        public WeekDayRulesetDefinition(WeekListData weekListData):base(
            WeekRulesetsDefinitions.Rules(weekListData.WeekConfig.GetDayBlob(weekListData.MainBlob).ParameterSet),
            WeekRulesetsDefinitions.DayConditions()
            )
        { }
    }
}
