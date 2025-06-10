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

namespace psdPH
{
    [Serializable]
    public class DowLayernamePair
    {
        public DayOfWeek Dow;
        public string Layername;
        public DowLayernamePair() { }
        public DowLayernamePair(DayOfWeek dow, string layername)
        {
            Dow = dow;
            Layername = layername;
        }
    }
    [Serializable]
    public class WeekConfig
    {
        
        [XmlIgnore]
        public Dictionary<DayOfWeek, string> DowPrototypeLayernameDict
        {
            get => DowPlaceholderLayernameList.ToDictionary(p => p.Dow, p => p.Layername); set
            {
                var result = new List<DowLayernamePair>();
                foreach (var item in value)
                    result.Add(new DowLayernamePair(item.Key, item.Value));
                DowPlaceholderLayernameList = result;
            }
        }
        public List<DowLayernamePair> DowPlaceholderLayernameList = new List<DowLayernamePair>();
        public DateFormat DayDateFormat;
        public DateFormat DowFormat;

        public string PrototypeLayerName;
        public string WeekDatesParameterName;
        public string DowParameterName;
        public string DateParameterName;
        StringParameter GetStringParameter(Blob blob, string name) => 
            blob.ParameterSet.GetByType<StringParameter>().First(_ => _.Name == name);
        internal StringParameter GetWeekDatesPar(Blob blob) => GetStringParameter(blob,WeekDatesParameterName);
        
        internal StringParameter GetDatePar(Blob blob) => GetStringParameter(blob, DateParameterName);
        
        internal StringParameter GetDowPar(Blob blob) => GetStringParameter(blob, DowParameterName);
        
        internal Blob GetDayBlob(Blob blob)=> GetDayPrototype(blob).Blob;
        
        internal PrototypeLeaf GetDayPrototype(Blob blob)
        {
            return blob.GetChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
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
