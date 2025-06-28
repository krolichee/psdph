using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Core
{
    [Localizator]
    public class ParametersLocalizator
    {
        public static void RegisterLocalizations()
        {
            TypeLocalization.RegisterLocalization(
            new Dictionary<Type, string>
        {
                { typeof(FlagParameter),"Флаг" },
                { typeof(StringParameter),"Строка" },
                { typeof(StringChooseParameter),"Выбор строки" },

        });
        }
    }
}
