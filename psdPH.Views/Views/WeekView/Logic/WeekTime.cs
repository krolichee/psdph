using System;

namespace psdPH.Views.WeekView.Logic
{
    public class WeekTime
    {
        public static int CurrentWeek => GetWeekFromUnixTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        public static int GetWeekFromUnixTime(long unixTime)
        {
            // Unix-время начинается с 01.01.1970 00:00:00 UTC
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Добавляем Unix-время к эпохе, чтобы получить текущую дату и время
            DateTime currentDate = epoch.AddSeconds(unixTime);

            // Первая неделя начинается с 05.01.1970
            DateTime firstWeekStart = new DateTime(1970, 1, 5, 0, 0, 0, DateTimeKind.Utc);

            // Если текущая дата раньше начала первой недели, возвращаем 0
            if (currentDate < firstWeekStart)
            {
                return 0;
            }

            // Вычисляем разницу в днях между текущей датой и началом первой недели
            TimeSpan timeSinceFirstWeek = currentDate - firstWeekStart;

            // Вычисляем номер недели, деля количество дней на 7 и добавляя 1
            int weekNumber = (int)(timeSinceFirstWeek.TotalDays / 7) + 1;

            return weekNumber;
        }
        public static DateTime GetDateByWeekAndDay(int weekNumber, DayOfWeek dayOfWeek)
        {
            // Начало первой недели: 05.01.1970 (понедельник)
            DateTime firstWeekStart = new DateTime(1970, 1, 5, 0, 0, 0, DateTimeKind.Utc);

            // Вычисляем начало указанной недели
            DateTime startOfWeek = firstWeekStart.AddDays((weekNumber - 1) * 7);

            // Корректируем день недели, чтобы неделя начиналась с понедельника
            int dayOffset = (int)dayOfWeek - (int)DayOfWeek.Monday;
            if (dayOffset < 0)
                dayOffset += 7; // Если день недели раньше понедельника (например, Sunday), добавляем 7 дней

            // Находим указанный день недели
            DateTime targetDate = startOfWeek.AddDays(dayOffset);

            return targetDate;
        }
    }

}
