using psdPH.TemplateEditor;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using Condition = psdPH.Logic.Rules.Condition;
using System;
using psdPH.RuleEditor;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public static class WeekRulesetsDefinitions
    {
        public static Rule[] Rules(Composition root) =>
            RuleDicts.Rules(root);
        [Obsolete]
        public static Condition[] WeekConditions(Composition root) => new Condition[]
        {
            new WeekCondition(root)
        };

        public static Condition[] DayConditions(Composition root) => new Condition[]
        {
            new EveryNDayCondition(root),
            new DayOfWeekCondition(root)
        };
    };
    public class DayRulesetDefinition: RulesetDefinition
    {
        public override Rule[] Rules => WeekRulesetsDefinitions.Rules(_root);
        public override Condition[] Conditions=> WeekRulesetsDefinitions.DayConditions(_root);
        public DayRulesetDefinition(Composition root) : base(root) { }
        

    }
}
