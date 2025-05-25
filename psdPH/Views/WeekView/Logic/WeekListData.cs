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
        public static WeekListData Create(WeekConfig weekConfig, Blob root)
        {
            var result = new WeekListData();
            result.WeekConfig = weekConfig;
            result.RootBlob = root;
            return result;
        }
        [XmlIgnore]
        public WeekConfig WeekConfig;
        public ObservableCollection<WeekData> Weeks = new ObservableCollection<WeekData>();
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
    }
}
