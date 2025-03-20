using psdPH.Logic.Compositions;
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
            public List<WeekData> Weeks = new List<WeekData>();
            public Blob RootBlob;
            public void Restore()
        {
            RootBlob.Restore();
            foreach (var week in Weeks)
            {
                week.MainBlob.Restore();
                foreach (var item in week.DowBlobList)
                {
                    item.Blob.Restore(week.MainBlob);
                }
            }
        }
            internal void NewWeek(WeekConfig weekDowsConfig,Blob rootBlobPrototype)
            {
            int week;
            if (Weeks.Any())
                week = Weeks.Max((WeekData w) => w.Week) + 1;
            else
                week = WeekTime.GetCurrentWeekFromUnixTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Weeks.Add(new WeekData(week, weekDowsConfig,rootBlobPrototype));
            }
        }
}
