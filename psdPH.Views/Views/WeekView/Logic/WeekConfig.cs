using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using psdPH.Logic.Rules;
using psdPH.Logic.Parameters;
using System.Globalization;
using psdPH.Parameters;

namespace psdPH
{
    [Serializable]
    public class DowGuidPair
    {
        public DayOfWeek Dow;
        public Guid Guid;
        public DowGuidPair() { }
        public DowGuidPair(DayOfWeek dow, Guid guid)
        {
            Dow = dow;
            Guid = guid;
        }
    }
    [Serializable]
    public class WeekConfig
    {
        
        [XmlIgnore]
        public Dictionary<DayOfWeek, Guid> DowPrototypeLayernameDict
        {
            get => DowPlaceholderLayernameList.ToDictionary(p => p.Dow, p => p.Guid); set
            {
                var result = new List<DowGuidPair>();
                foreach (var item in value)
                    result.Add(new DowGuidPair(item.Key, item.Value));
                DowPlaceholderLayernameList = result;
                
            }
        }
        public List<DowGuidPair> DowPlaceholderLayernameList = new List<DowGuidPair>();
        public DateFormat DayDateFormat;
        public DateFormat DowFormat;

        public string PrototypeLayerName;
        public string WeekDatesParameterName;
        public string DowParameterName;
        public string DateParameterName;
        StringParameter GetStringParameter(RootBlob blob, string name) => 
            blob.ParameterSet.GetByType<StringParameter>().First(_ => _.Name == name);
        internal StringParameter GetWeekDatesPar(RootBlob blob) => GetStringParameter(blob,WeekDatesParameterName);
        
        internal StringParameter GetDatePar(RootBlob blob) => GetStringParameter(blob, DateParameterName);
        
        internal StringParameter GetDowPar(RootBlob blob) => GetStringParameter(blob, DowParameterName);
        
        internal PrototypeBlob GetDayPrototype(Composition blob)
        {
            return blob.GetChildren<PrototypeBlob>().First(p => p.LayerName == PrototypeLayerName);
        }

        //public void FillDateAndDow(DowBlob dayBlob)
        //{
        //    var week = dayBlob.Week;
        //    var dow = dayBlob.Dow;
        //    var dateTime = WeekTime.GetDateByWeekAndDay(week, dow);
        //    var dateTextLeaf = GetDatePar(dayBlob);
        //    var dowTextLeaf = GetDowPar(dayBlob);
        //    dateTextLeaf.Text = DayDateFormat.Format(dateTime);
        //    dowTextLeaf.Text = DowFormat.Format(dateTime);
        //}
        public void FillDateAndDow(DayParameterSet parameters)
        {
            FillDateAndDow(parameters, parameters.Week,parameters.Dow);
        }
        public void FillDateAndDow(ParameterSet parameters,int week,DayOfWeek dow)
        {
            var dateTime = WeekTime.GetDateByWeekAndDay(week, dow);
            FillDateAndDow(parameters, dateTime);
        }
        public void FillDateAndDow(ParameterSet parameters,DateTime dateTime)
        {
            parameters.Set(DateParameterName, DayDateFormat.Format(dateTime));
            parameters.Set(DowParameterName, DowFormat.Format(dateTime));
        }
        public void FillWeekDate(ParameterSet parameters,int week)
        {
            parameters.Set(WeekDatesParameterName, GetWeekDatesString(week));
        }

        internal string GetWeekDatesString(int week)
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
