namespace psdPH.Localization
{
    using System;
    using System.Collections.Generic;

    public static class TypeLocalization
    {
        private static readonly Dictionary<Type, string> Localizations = new Dictionary<Type, string>();
        public static void RegisterLocalization(Dictionary<Type, string> localization)
        {
            foreach (var pair in localization)
                Localizations[pair.Key] = pair.Value;
        }
        internal static string GetLocalizedDescription(this Type type)
        {
            if (Localizations.TryGetValue(type, out var description))
            {
                return description;
            }
            return type.ToString();
        }
    }
}
