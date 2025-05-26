using psdPH.Logic;
using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.WeekConfig;

namespace psdPH.Views.WeekView.Logic
{
    public class EveryNDayCondition : Condition
    {
        public DateTime StartDateTime;
        public int Interval;
        public override Parameter[] Setups => throw new NotImplementedException();

        public EveryNDayCondition(Composition composition) : base(composition) { }
        public override bool IsValid()
        {
            var dayBlob = Composition as DowBlob;
            var dateTime = WeekTime.GetDateByWeekAndDay(dayBlob.Week, dayBlob.Dow);
            TimeSpan timeSinceFirstWeek = dateTime - StartDateTime;
            return timeSinceFirstWeek.TotalDays % Interval == 0;
        }
        public EveryNDayCondition() : base(null) { }
    }
    public class DayOfWeekCondition : Condition
    {
        public DayOfWeek DayOfWeek;

        public override Parameter[] Setups => throw new NotImplementedException();

        public DayOfWeekCondition(Composition composition) : base(composition) { }
        public override bool IsValid()
        {
            var dayBlob = Composition as DowBlob;
            return dayBlob.Dow == DayOfWeek;
        }
        public DayOfWeekCondition() : base(null) { }
    }
    public class WeekCondition : Condition
    {
        public int Week;
        public WeekCondition(Composition composition) : base(composition) { }
        public override Parameter[] Setups => throw new NotImplementedException();
        public override bool IsValid()
        {
            return (Composition as WeekBlob).Week == Week;
        }
    }
}
