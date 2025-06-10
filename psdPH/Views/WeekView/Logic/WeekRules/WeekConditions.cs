using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.WeekConfig;

namespace psdPH.Views.WeekView.Logic
{
    public interface ParameterSetCondition
    {
        void SetParameterSet(ParameterSet parset);
    }
    public class EveryNDayCondition : Condition, ParameterSetCondition
    {
        DayParameterSet ParameterSet;
        public override string ToString() => "каждый 'n' день";
        public DateTime? StartDateTime;
        public int Interval=0;
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var intervalConfig = new SetupConfig(this, nameof(Interval), "каждый...");
                var intervalParameter = Setup.IntInput(intervalConfig,1,366);
                
                var startDateConfig = new SetupConfig(this, nameof(StartDateTime), "начиная с");
                var startDateParameter = Setup.Date(startDateConfig);

                result.Add(intervalParameter);
                result.Add(startDateParameter);
                return result.ToArray();
            }
        }

        //public EveryNDayCondition(Composition composition) : base(composition) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&StartDateTime!=null&&Interval!=0;
        }
        public override bool IsValid()
        {
            var week = ParameterSet.Week;
            var dow = ParameterSet.Dow;
            DateTime startDateTime =(DateTime)StartDateTime;
            var dateTime = WeekTime.GetDateByWeekAndDay(week, dow);
            TimeSpan timeSinceFirstWeek = dateTime - startDateTime;
            return timeSinceFirstWeek.TotalDays % Interval == 0;
        }

        public void SetParameterSet(ParameterSet parset)
        {
            ParameterSet = parset as DayParameterSet;
            
        }

        public EveryNDayCondition(DayParameterSet parset) : base(null)
        {
            ParameterSet = parset;
        }
        public EveryNDayCondition() : base(null) { }
    }
    public class DayOfWeekCondition : Condition
    {
        public DayParameterSet ParameterSet;
        public override string ToString() => "день недели";
        public DayOfWeek DayOfWeek;

        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var dowConfig = new SetupConfig(this, nameof(DayOfWeek), "");
                var dowParameter = Setup.EnumChoose(dowConfig, typeof(DayOfWeek));
                result.Add(dowParameter);
                return result.ToArray();
            }
        }

        public DayOfWeekCondition(Composition composition) : base(composition) { }
        public override bool IsValid()
        {
            return ParameterSet.Dow == DayOfWeek;
        }
        public DayOfWeekCondition() : base(null) { }
    }
    [Obsolete]
    public class WeekCondition : Condition
    {
        DayParameterSet ParameterSet;
        public override string ToString() => "неделя";
        public int Week;
        public WeekCondition(Composition composition) : base(composition) { }
        public override Setup[] Setups
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
            return ParameterSet.Week == Week;
        }
        public WeekCondition() : base(null) { }
    }
}
