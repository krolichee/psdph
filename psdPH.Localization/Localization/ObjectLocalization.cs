namespace psdPH
{
    using System;
    using System.Collections.Generic;

    public static class ObjectLocalization
    {
        private static readonly Dictionary<Type, string> Localizations = new Dictionary<Type, string>();
        public static void RegisterLocalization(Dictionary<Type, string> localization)
        {
            foreach (var pair in localization)
                Localizations[pair.Key] = pair.Value;
        }
        internal static string GetLocalizedDescription(object obj)
        {
            if (Localizations.TryGetValue(obj?.GetType(), out var description))
            {
                return description;
            }
            return obj?.ToString();
        }
    }
}
