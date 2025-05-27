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
        public override string ToString() => "каждый 'n' день";
        public DateTime StartDateTime=new DateTime(1970,1,1);
        public int Interval=1;
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var intervalConfig = new ParameterConfig(this, nameof(Interval), "каждый...");
                var intervalParameter = Parameter.IntInput(intervalConfig,1,366);
                
                var startDateConfig = new ParameterConfig(this, nameof(StartDateTime), "начиная с");
                var startDateParameter = Parameter.Calendar(startDateConfig);

                result.Add(intervalParameter);
                result.Add(startDateParameter);
                return result.ToArray();
            }
        }

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
        public override string ToString() => "день недели";
        public DayOfWeek DayOfWeek;

        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var dowConfig = new ParameterConfig(this, nameof(DayOfWeek), "");
                var dowParameter = Parameter.EnumChoose(dowConfig, typeof(DayOfWeek));
                result.Add(dowParameter);
                return result.ToArray();
            }
        }

        public DayOfWeekCondition(Composition composition) : base(composition) { }
        public override bool IsValid()
        {
            var dayBlob = Composition as DowBlob;
            return dayBlob.Dow == DayOfWeek;
        }
        public DayOfWeekCondition() : base(null) { }
    }
    [Obsolete]
    public class WeekCondition : Condition
    {
        public override string ToString() => "неделя";
        public int Week;
        public WeekCondition(Composition composition) : base(composition) { }
        public override Parameter[] Setups
        {
            get
            {
                throw new NotImplementedException();
                //var result = new List<Parameter>();
                //var dowConfig = new ParameterConfig(this, nameof(Week), "");
                //var dowParameter = Parameter.EnumChoose(dowConfig, typeof(DayOfWeek));
                //result.Add(dowParameter);
                //return result.ToArray();
            }
        }
        public override bool IsValid()
        {
            return (Composition as WeekBlob).Week == Week;
        }
        public WeekCondition() : base(null) { }
    }
}
