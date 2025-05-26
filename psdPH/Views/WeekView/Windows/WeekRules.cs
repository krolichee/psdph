using psdPH.TemplateEditor;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using Condition = psdPH.Logic.Rules.Condition;
using System;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public static class WeekRules
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
}
