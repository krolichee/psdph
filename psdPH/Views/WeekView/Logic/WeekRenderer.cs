using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Views.WeekView.Logic
{
    class WeekRenderer
    {
        public static void renderWeek(WeekData weekData, Document doc)
        {
            WeekConfig weekConfig = weekData.WeekConfig;
            DowLayernamePair whereLayernameIs(string layername, List<DowLayernamePair> pairs)
            {
                return pairs.First(dl_p => dl_p.Layername == layername);
            }
            DayOfWeek getMatchingDow(PlaceholderLeaf p)
            {
                var pairs = weekConfig.DowPrototypeLayernameList;
                return whereLayernameIs(p.LayerName, pairs).Dow;
            }
            
            WeekData weekData_clone = weekData.Clone();
            PlaceholderLeaf[] prototypes = weekData_clone.MainBlob.getChildren<PlaceholderLeaf>();
            Dictionary<DayOfWeek, PlaceholderLeaf> dowPlaceholderDict = prototypes.ToDictionary(getMatchingDow, p => p);
            var dayOfWeekEnumValues = Enum.GetValues(typeof(DayOfWeek)).Cast<Enum>();
            foreach (DayOfWeek dow in dayOfWeekEnumValues)
            {
                var ph = dowPlaceholderDict[dow];
                var blob = weekData_clone.DowBlobsDict[dow];
                ph.ReplaceWithFiller(doc, blob);
            }
            weekData_clone.MainBlob.Apply(doc);
        }
    }
}
