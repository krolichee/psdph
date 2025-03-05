using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    using Photoshop;
    using psdPH.Logic;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public static class EnumLocalization
    {
        private static readonly Dictionary<Type, Dictionary<object, string>> Localizations = new Dictionary<Type, Dictionary<object, string>>()
    {
        {
            typeof(PsJustification), new Dictionary<object, string>
            {
                { PsJustification.psLeft, "слева" },
                { PsJustification.psCenter, "по центру" },
                { PsJustification.psRight, "справа" }
            }
        },
        {
            typeof(ChangeMode), new Dictionary<object, string>
            {
                { ChangeMode.Abs, "изменить на" },
                { ChangeMode.Rel, "установить" }
            }
        },
        {
            typeof(DayOfWeek), new Dictionary<object, string>
            {
                { DayOfWeek.Monday, "Пн"},
                { DayOfWeek.Tuesday, "Вт"},
                { DayOfWeek.Wednesday, "Ср"},
                { DayOfWeek.Thursday, "Чт"},
                { DayOfWeek.Friday, "Пт"},
                { DayOfWeek.Saturday, "Сб"},
                { DayOfWeek.Sunday, "Вс"},
            }
        },
        {
            typeof(Composition), new Dictionary<object, string>
            {
                {typeof(FlagLeaf), "Флаг"},
                {typeof(TextLeaf), "Текст" }
            
            }
        }
    };
    public static string GetLocalizedDescription<TEnum>(TEnum value)
    {
        Type enumType = value.GetType();

        if (Localizations.TryGetValue(enumType, out var localization) &&
            localization.TryGetValue(value, out var description))
        {
            return description;
        }

        // Если локализация не найдена, возвращаем строковое представление значения
        return value.ToString();
    }

}
}
