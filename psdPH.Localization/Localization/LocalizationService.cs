using System;
using System.CodeDom;
using System.Reflection;
namespace psdPH.Localization
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class LocalizatorAttribute : Attribute { }
    public static class LocalizationService
    {
        static bool initialized;
        public static string Localize(this object obj)
        {
            if (!initialized)
            {
                InitializeLocalizations();
                initialized = true;
            }
            if (obj is Enum)
                return EnumLocalization.GetLocalizedDescription(obj as Enum);
            else if (obj is bool)
                return BoolLocalization.LocalizeBool((bool)obj);
            else if (obj is Type)
                return TypeLocalization.GetLocalizedDescription(obj as Type);
            else
                return ObjectLocalization.GetLocalizedDescription(obj);
        }
        public static void InitializeLocalizations()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttribute<LocalizatorAttribute>() != null
                            && type.IsAbstract && type.IsSealed) // Проверяем, что это статический класс
                        {
                            var registerMethod = type.GetMethod("RegisterLocalizations",
                                BindingFlags.Public | BindingFlags.Static);

                            registerMethod?.Invoke(null, null);
                        }
                    }
                }
                catch (ReflectionTypeLoadException) { }
            }
        }
    }
}
