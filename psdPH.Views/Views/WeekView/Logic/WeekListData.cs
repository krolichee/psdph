using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Views;
using psdPH.Views.SimpleView.Logic;
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
    public class WeekListData:ViewListData
    {
        [XmlIgnore]
        public WeekConfig WeekConfig;
        public ObservableCollection<WeekData> Weeks = new ObservableCollection<WeekData>();
        public static WeekListData CreateWeekListData(WeekConfig weekConfig,WeekRulesets weekRulesets, RootBlob root)
        {
            var result = new WeekListData();
            result.WeekConfig = weekConfig;
            result.RootBlob = root;
            result.WeekRulesets = weekRulesets;
            return result;
        }
        public static WeekListData CreateWeekListData(WeekConfig weekConfig, RootBlob root)
        {
            var weekRulesets = new WeekRulesets();
            return CreateWeekListData(weekConfig, weekRulesets, root);
        }
        [XmlIgnore]
        public WeekRulesets WeekRulesets = new WeekRulesets();
        public void Restore()
        {
            RootBlob.Restore();
            foreach (var week in Weeks)
                week.Restore(this);
        }
        public override void New()
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
        public WeekListData() { }
    }
}
