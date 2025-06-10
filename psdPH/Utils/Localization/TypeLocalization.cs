namespace psdPH
{
    using psdPH.Logic;
    using psdPH.Logic.Compositions;
    using psdPH.Logic.Parameters;
    using System;
    using System.Collections.Generic;

    public static class TypeLocalization
    {
        private static readonly Dictionary<Type, string> Localizations = new Dictionary<Type, string>
        {
                {typeof(Blob), "Поддокумент" },

                {typeof(PrototypeLeaf), "Прототип" },
                {typeof(PlaceholderLeaf), "Заглушка" },

                {typeof(ImageLeaf), "Изображение" },
                {typeof(TextLeaf), "Текст" },
                {typeof(LayerLeaf), "Слой" },
                {typeof(GroupLeaf), "Группа" },
                {typeof(AreaLeaf),"Зона" },

                {typeof(Rule), "Правило" },

                {typeof(FlagParameter),"Флаг" },
                {typeof(StringParameter),"Строка" },

        };
        public static string GetLocalizedDescription(this Type type)
        {
            if (Localizations.TryGetValue(type, out var description))
            {
                return description;
            }
            return type.ToString();
        }
    }
}
