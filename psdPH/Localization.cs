using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    using Photoshop;
    using System;
    using System.Collections.Generic;
    using System.Windows;


    public static class EnumLocalization
    {
        private static readonly Dictionary<Type, Dictionary<object, string>> Localizations = new Dictionary<Type, Dictionary<object, string>>()
    {
        {
            typeof(Status), new Dictionary<object, string>
            {
                { Status.NotStarted, "Не начат" },
                { Status.InProgress, "В процессе" },
                { Status.Completed, "Завершён" }
            }
        },
        {
            typeof(Priority), new Dictionary<object, string>
            {
                { Priority.Low, "Низкий" },
                { Priority.Medium, "Средний" },
                { Priority.High, "Высокий" }
            }
        }
    };
        public static string GetLocalizedDescription<TEnum>(TEnum value) where TEnum : Enum
        {
            var enumType = typeof(TEnum);

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
