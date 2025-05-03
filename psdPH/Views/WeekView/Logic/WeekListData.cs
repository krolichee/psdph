using psdPH.Logic.Compositions;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
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
        public List<WeekData> Weeks = new List<WeekData>();
        public Blob RootBlob;
        public void Restore()
        {
            RootBlob.Restore();
            foreach (var week in Weeks)
            {
                week.Restore(this);
                foreach (var item in week.DowBlobList)
                {
                    item.Blob.Restore(week.MainBlob);
                }
            }
        }
        internal void NewWeek()
        {
            int new_week;
            if (Weeks.Any())
                new_week = Weeks.Max((WeekData w) => w.Week) + 1;
            else
                new_week = WeekTime.GetCurrentWeekFromUnixTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var new_weekData = new WeekData(new_week, this);
            
            Weeks.Add(new_weekData);
        }
    }
}
