using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Rules
{
    [Localizator]
    public class RulesLocalizator
    {
        public static void RegisterLocalizations()
        {
            TypeLocalization.RegisterLocalization(
            new Dictionary<Type, string>
        {
                { typeof(CompositionRule), "Правило" }

        });
            EnumLocalization.RegisterLocalization(new Dictionary<ChangeMode, string>
            {
                { ChangeMode.Abs, "установить" },
                { ChangeMode.Rel, "изменить на" }
            });
        }
    }
}
