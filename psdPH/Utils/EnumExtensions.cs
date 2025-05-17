using System;

namespace psdPH.Logic
{
    //------------------
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return EnumLocalization.GetLocalizedDescription(value);
        }
        public static string ToString(this Enum value)
        {
            return value.GetDescription();
        }
    }


}
