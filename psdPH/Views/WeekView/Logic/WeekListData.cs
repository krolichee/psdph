using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    [XmlInclude(typeof(Blob))]
    public class WeekListData
    {
        public RuleSet DayRules = new RuleSet();
        public RuleSet WeekRules = new RuleSet();
        public static WeekListData Create(WeekConfig weekConfig, Blob root)
        {
#pragma warning disable CS0612 // Тип или член устарел
            var result = new WeekListData();
#pragma warning restore CS0612 // Тип или член устарел
            result.WeekConfig = weekConfig;
            result.RootBlob = root;
            result.DayRules.Composition = root;
            result.WeekRules.Composition = weekConfig.GetDayPrototype(root).Blob;
            return result;
        }
        [XmlIgnore]
        public WeekConfig WeekConfig;
        public ObservableCollection<WeekData> Weeks = new ObservableCollection<WeekData>();
        [XmlIgnore]
        public Blob RootBlob;
        public void Restore()
        {
            RootBlob.Restore();
            foreach (var week in Weeks)
                week.Restore(this);
        }
        public void NewWeek()
        {
            int new_week;
            if (Weeks.Any())
                new_week = Weeks.Max((WeekData w) => w.Week) + 1;
            else
                new_week = WeekTime.CurrentWeek;
            NewWeek(new_week);
        }
        public void NewWeek(int new_week)
        {
            var new_weekData = new WeekData(new_week, this);
            Weeks.Add(new_weekData);
        }
        public void InjectDayRules(DowBlob dayBlob)
        {
            foreach (var item in DayRules.Rules)
                dayBlob.RuleSet.AddRule(item.Clone());
        }
        internal void InjectWeekRules(WeekData weekData)
        {
            foreach (var item in WeekRules.Rules)
                weekData.MainBlob.RuleSet.AddRule(item.Clone());
        }
        internal void InjectRules(WeekData weekData)
        {
            foreach (var item in weekData.DowBlobList)
                InjectDayRules(item);
            InjectWeekRules(weekData);
        }
        [Obsolete]
        public WeekListData() { }
    }
}
