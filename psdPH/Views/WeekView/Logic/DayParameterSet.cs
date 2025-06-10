using psdPH.Logic.Parameters;
using psdPH.Utils;
using System;
using System.Xml.Serialization;

namespace psdPH.Views.WeekView
{
    [Serializable]
    public class DayParameterSet:ParameterSet
    {
        [XmlAttribute("Week")]
        public int Week;
        [XmlAttribute("Dow")]
        public DayOfWeek Dow;
        public static DayParameterSet FromParset(ParameterSet parset, DayOfWeek dow, int week){
            var result = new DayParameterSet(dow,week);
            foreach (var par in parset.AsCollection())
                result.AsCollection().Add(par.Clone());
            return result;
            //DayParameterSet result = CloneConverter.Convert<DayParameterSet>(parset);
            //result.Dow = dow;
            //result.Week = week;
            //return result;
        }

        protected DayParameterSet(ParameterSet parset, DayOfWeek dow,int week) :this(dow,week)
        {
            foreach (var par in parset.AsCollection())
                AsCollection().Add(CloneConverter.Clone(par) as Parameter);
        }
        public DayParameterSet(DayOfWeek dow, int week)
        {
            Dow = dow;
            Week = week;
        }
        public DayParameterSet() { }
    }
}
