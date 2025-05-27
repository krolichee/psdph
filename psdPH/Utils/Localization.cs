namespace psdPH
{
    using global::Photoshop;
    using Photoshop;
    using psdPH.Logic;
    using psdPH.Logic.Compositions;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public static class TypeLocalization
    {
        private static readonly Dictionary<Type, string> Localizations = new Dictionary<Type, string>
        {
                {typeof(Blob), "Поддокумент" },

                {typeof(FlagLeaf), "Флаг"},
                {typeof(PrototypeLeaf), "Прототип" },
                {typeof(PlaceholderLeaf), "Заглушка" },

                {typeof(ImageLeaf), "Изображение" },
                {typeof(TextLeaf), "Текст" },
                {typeof(LayerLeaf), "Слой" },
                {typeof(GroupLeaf), "Группа" },
                {typeof(AreaLeaf),"Зона" },

                {typeof(Rule), "Правило" },

        };
        public static string GetLocalizedDescription(Type type)
        {
            if (Localizations.TryGetValue(type, out var description))
            {
                return description;
            }
            return type.ToString();
        }
    }
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
