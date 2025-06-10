using psdPH.TemplateEditor;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using Condition = psdPH.Logic.Rules.Condition;
using System;
using psdPH.RuleEditor;
using System.Linq;
using psdPH.Logic.Rules;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    static class WeekRulesetsDefinitions
    {
        public static Rule[] Rules(Composition root) =>
            new StructureRulesetDefinition(root).Rules.Concat(
                new Rule[]
                {
                    new FlagRule(root)
                }
                ).ToArray();
        [Obsolete]
        public static Condition[] WeekConditions(Composition root) => new Condition[]
        {
            new WeekCondition(root)
        };

        public static Condition[] DayConditions(Composition root) => new Condition[]
        {
            new EveryNDayCondition(null),
            new DayOfWeekCondition(root)
        };
    };
    [Obsolete]
    public class WeekRulesetDefinition: RulesetDefinition
    {
        public WeekRulesetDefinition(Composition root) : base(
            WeekRulesetsDefinitions.Rules(root),
            WeekRulesetsDefinitions.WeekConditions(root)
            )
        { }
    }
    public class DayRulesetDefinition: RulesetDefinition
    {
        public DayRulesetDefinition(Composition root):base(
            WeekRulesetsDefinitions.Rules(root),
            WeekRulesetsDefinitions.DayConditions(root)
            )
        { }
    }
}
