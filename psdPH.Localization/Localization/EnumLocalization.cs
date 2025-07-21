using System;
using System.Collections.Generic;
namespace psdPH
{
   
    public class EnumWrapper
    {
        public Enum Value;
        public EnumWrapper(Enum value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Localization.LocalizationService.Localize(Value);
        }
    }
    public static class EnumLocalization
    {
        private static readonly Dictionary<Type, Dictionary<object, string>> Localizations = new Dictionary<Type, Dictionary<object, string>>();
        public static void RegisterLocalization<TEnum>(Dictionary<TEnum, string> localization)
        where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var dict = new Dictionary<object, string>();

            foreach (var pair in localization)
            {
                dict[pair.Key] = pair.Value;
            }

            Localizations[enumType] = dict;
        }
        internal static string GetLocalizedDescription<TEnum>(this TEnum value)
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
