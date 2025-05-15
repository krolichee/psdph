using psdPH.Logic.Compositions;
using psdPH.Views;
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
    public class WeekListData:ViewListData<WeekData>
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
        public override void New()
        {
            int new_week;
            if (Variants.Any())
                new_week = Variants.Max((WeekData w) => w.Week) + 1;
            else
                new_week = WeekTime.GetCurrentWeekFromUnixTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var new_weekData = new WeekData(new_week, this);
            Variants.Add(new_weekData);
        }
    }
}
