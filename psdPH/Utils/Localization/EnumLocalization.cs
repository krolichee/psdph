namespace psdPH
{
    using global::Photoshop;
    using psdPH.Logic;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using static psdPH.Photoshop.LayerWr;

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
                { ChangeMode.Abs, "установить" },
                { ChangeMode.Rel, "изменить на" }
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
            typeof(HorizontalAlignment), new Dictionary<object, string>
            {
                { HorizontalAlignment.Left, "слева" },
                { HorizontalAlignment.Center, "по центру" },
                { HorizontalAlignment.Right, "справа" }
            }
        },
    {
            typeof(VerticalAlignment), new Dictionary<object, string>
            {
                { VerticalAlignment.Top, "cверху" },
                { VerticalAlignment.Center, "по центру" },
                { VerticalAlignment.Bottom, "снизу" }
            }
        },
    {
            typeof(HAilgnment), new Dictionary<object, string>
            {
                { HAilgnment.Left, "слева" },
                { HAilgnment.Center, "по центру" },
                { HAilgnment.Right, "справа" },
                { HAilgnment.None, "не выравнивать" }
            }
        },
    {
            typeof(VAilgnment), new Dictionary<object, string>
            {
                { VAilgnment.Top, "cверху" },
                { VAilgnment.Center, "по центру" },
                { VAilgnment.Bottom, "снизу" },
                { VAilgnment.None, "не выравнивать" }
            }
        },
    {
            typeof(ConsiderFx), new Dictionary<object, string>
            {
                { ConsiderFx.WithFx, "с эффектами" },
                { ConsiderFx.NoFx, "без эффектов" }
            }
        }
    };

        public static string GetLocalizedDescription<TEnum>(this TEnum value)
        {
            Type enumType = value.GetType();

            if (Localizations.TryGetValue(enumType, out var localization) &&
                localization.TryGetValue(value, out var description))
            {
                return description;
            }
            return value.ToString();
        }

    }
}
