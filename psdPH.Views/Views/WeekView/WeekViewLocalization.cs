using psdPH.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.Views.WeekView
{
    [Localizator]
    public class WeekViewLocalization
    {
        public static void RegisterLocalizations()
        {
            EnumLocalization.RegisterLocalization(new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Пн"},
                { DayOfWeek.Tuesday, "Вт"},
                { DayOfWeek.Wednesday, "Ср"},
                { DayOfWeek.Thursday, "Чт"},
                { DayOfWeek.Friday, "Пт"},
                { DayOfWeek.Saturday, "Сб"},
                { DayOfWeek.Sunday, "Вс"},
            });
        }
        
    }
}
