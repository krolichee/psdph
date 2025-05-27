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
        public DateFormat DayDowFormat;

        public string TilePreviewTextLeafName;
        public string PrototypeLayerName;
        public string WeekDatesTextLeafName;
        public string DowTextLeafLayerName;
        public string DateTextLeafLayerName;
        internal TextLeaf GetWeekDatesTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == WeekDatesTextLeafName);
        }
        internal TextLeaf GetDateTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == DateTextLeafLayerName);
        }
        internal TextLeaf GetDowTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == DowTextLeafLayerName);
        }
        internal Blob GetDayBlob(Blob blob)
        {
            return GetDayPrototype(blob).Blob;
        }

        internal PrototypeLeaf GetDayPrototype(Blob blob)
        {
            return blob.getChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
        }
        internal TextLeaf GetTilePreviewTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(p => p.LayerName == TilePreviewTextLeafName);
        }

        public void FillDateAndDow(DowBlob dayBlob)
        {
            var week = dayBlob.Week;
            var dow = dayBlob.Dow;
            var dateTime = WeekTime.GetDateByWeekAndDay(week, dow);
            var dateTextLeaf = GetDateTextLeaf(dayBlob);
            var dowTextLeaf = GetDowTextLeaf(dayBlob);
            dateTextLeaf.Text =DayDateFormat.Format(dateTime);
            dowTextLeaf.Text = DayDowFormat.Format(dateTime);
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
