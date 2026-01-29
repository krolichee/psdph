using psdPH.Localization;
using psdPH.Logic;
using psdPH.Logic.Ruleset.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Rules
{
    [Localizator]
    public static class RulesLocalizator
    {
        public static void RegisterLocalizations()
        {
            TypeLocalization.RegisterLocalization(
            new Dictionary<Type, string>
        {
                { typeof(CompositionRule), "Правило" },
                { typeof(AlignRule), "Выровнять по зоне" },
                { typeof(FitRule), "Вместить зоне" }

        });
            EnumLocalization.RegisterLocalization(new Dictionary<ChangeMode, string>
            {
                { ChangeMode.Abs, "установить" },
                { ChangeMode.Rel, "изменить на" }
            });
        }
    }
}
