using psdPH.Views.WeekView.Logic;
using System;

namespace psdPH.Views.WeekView
{
    public static class WeekDatesStrings
    {
        public static string getShortWeekDatesString(int week)
        {
            DateTime monday = WeekTime.GetDateByWeekAndDay(week, DayOfWeek.Monday);
            DateTime sunday = WeekTime.GetDateByWeekAndDay(week, DayOfWeek.Sunday);
            return monday.ToString("dd.MM") + " - " + sunday.ToString("dd.MM");
        }
        public static string getWeekDatesString(int week)
        {
            string result = "";
            DateTime monday = WeekTime.GetDateByWeekAndDay(week, DayOfWeek.Monday);
            DateTime sunday = WeekTime.GetDateByWeekAndDay(week, DayOfWeek.Sunday);
            if (monday.Month != sunday.Month)
                result = monday.ToString("dd MMMM") + " - " + sunday.ToString("dd MMMM");
            else
                result = monday.ToString("dd") + " - " + sunday.ToString("dd MMMM");
            return result;
        }
    }
}
