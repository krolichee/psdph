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
        
        public static WeekListData Create(WeekConfig weekConfig,WeekRulesets weekRulesets, Blob root)
        {
#pragma warning disable CS0612 // Тип или член устарел
            var result = new WeekListData();
#pragma warning restore CS0612 // Тип или член устарел
            result.WeekConfig = weekConfig;
            result.MainBlob = root;
            result.WeekRulesets = weekRulesets;
            result.WeekRulesets.Restore(root,weekConfig);
            return result;
        }
        public static WeekListData Create(WeekConfig weekConfig, Blob root)
        {
            var weekRulesets = new WeekRulesets();
#pragma warning disable CS0612 // Тип или член устарел
            var result = new WeekListData();
#pragma warning restore CS0612 // Тип или член устарел
            result.WeekConfig = weekConfig;
            result.MainBlob = root;
            result.WeekRulesets = weekRulesets;
            result.WeekRulesets.Restore(root, weekConfig);
            return result;
        }
        [XmlIgnore]
        public WeekRulesets WeekRulesets = new WeekRulesets();
        [XmlIgnore]
        public WeekConfig WeekConfig;
        public ObservableCollection<WeekData> Weeks = new ObservableCollection<WeekData>();
        [XmlIgnore]
        public Blob MainBlob;
        public void Restore()
        {
            WeekRulesets.Restore(MainBlob, WeekConfig);
            MainBlob.Restore();
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
        [Obsolete]
        public WeekListData() { }
    }
}
