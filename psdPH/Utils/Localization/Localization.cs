namespace psdPH
{
    using Photoshop;
    using System;
    using System.Windows.Media.Animation;
    public static class Localization
    {
        public static string LocalizeObj(this object obj)
        {
            if (obj is Enum)
                return EnumLocalization.GetLocalizedDescription(obj as Enum);
            else if (obj is bool)
                return BoolLocalization.LocalizeBool((bool)obj);
            else if (obj is Type)
                return TypeLocalization.GetLocalizedDescription(obj as Type);
            else
                return obj?.ToString();
        }
    }
    public static class BoolLocalization
    {
        public static string LocalizeBool(bool val) => val ? "истина":"ложь";
    }
}
