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
        public ObservableCollection<ConditionRule> DayRules=new ObservableCollection<ConditionRule>();
        public ObservableCollection<ConditionRule> WeekRules=new ObservableCollection<ConditionRule>();
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
        internal DateFormat DayDowFormat;

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
        internal PrototypeLeaf GetDayPrototype(Blob blob)
        {
            return blob.getChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
        }
        internal TextLeaf GetTilePreviewTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(p => p.LayerName == TilePreviewTextLeafName);
        }
        [Serializable]
        public abstract class WeekDataCondition : Condition
        {
            protected WeekDataCondition(Composition composition) : base(composition) { }
            [XmlIgnore]
            public override Parameter[] Setups => throw new NotImplementedException();
        }
        
        public class EveryNDayCondition : WeekDataCondition
        {
            public DateTime StartDateTime;
            public int Interval;
            public EveryNDayCondition(Composition composition) : base(composition) { }
            public override bool IsValid()

            { var dayBlob = Composition as DayBlob;
                var dateTime = WeekTime.GetDateByWeekAndDay(dayBlob.Week, dayBlob.Dow);
                TimeSpan timeSinceFirstWeek = dateTime - StartDateTime;
                return timeSinceFirstWeek.TotalDays % Interval == 0;
            }
            public EveryNDayCondition() :base(null) { }
        }

        public void IncludeDayRules(DayBlob dayBlob)
        {
            foreach (var item in DayRules)
            {
                dayBlob.RuleSet.AddRule(item.Clone());
            }
            
        }

        internal void IncludeRules(WeekData weekData_clone)
        {
            foreach (var item in weekData_clone.DowBlobList)
            {
                IncludeDayRules(item.DayBlob as DayBlob);
            }
            foreach (var item in WeekRules)
            {
                weekData_clone.MainBlob.RuleSet.AddRule(item.Clone());
            }
        }
        public void FillDateAndDow(DayBlob dayBlob)
        {
            var week = dayBlob.Week;
            var dow = dayBlob.Dow;
            var dateTime = WeekTime.GetDateByWeekAndDay(week, dow);
            var dateTextLeaf = GetDateTextLeaf(dayBlob);
            var dowTextLeaf = GetDowTextLeaf(dayBlob);
            dateTextLeaf.Text =DayDateFormat.Format(dateTime);
            dowTextLeaf.Text = DayDowFormat.Format(dateTime);
        }

    }
}
