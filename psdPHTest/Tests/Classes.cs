using psdPH.Logic.Compositions;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Tests
{
    public class WeekViewTest
    {
        public static DowLayernamePair GetPair(DayOfWeek dow) => new DowLayernamePair(dow,Localization.LocalizeObj(dow));
        public static DowLayernamePair[] DowLayernamePairs => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => GetPair(e)).ToArray();
        public static string[] DayOfWeekNames => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => Localization.LocalizeObj(e)).ToArray();
        public static WeekConfig GetWeekConfig()
        {
            return new WeekConfig()
            {
                DateTextLeafLayerName = "Число",
                DayDateFormat = new NoZeroDateFormat(),
                DowFormat = new ShortDowFormat().Lower,
                DowPlaceholderLayernameList = DowLayernamePairs.ToList(),
                DowTextLeafLayerName = "День недели",
                WeekDatesTextLeafName = "Даты недели",
                PrototypeLayerName = "Прототип дня",
                TilePreviewTextLeafName = "День недели"
            };
        }
        public static Blob GetBlob()
        {
            var blob = Blob.PathBlob("C:\\ProgramData\\psdPH\\Projects\\№пример\\template.psd");
            var dayBlob = Blob.LayerBlob("Прототип дня");
            dayBlob.AddChild(new TextLeaf() { LayerName = "Число" });
            dayBlob.AddChild(new TextLeaf() { LayerName = "День недели" });
            var dayPrototype = new PrototypeLeaf() { Blob = dayBlob, RelativeLayerName = "Пн", LayerName = "Прототип дня" };
            blob.AddChild(dayBlob);
            blob.AddChild(dayPrototype);
            foreach (var dow in DayOfWeekNames)
                blob.AddChild(new PlaceholderLeaf() { Prototype = dayPrototype, LayerName = dow });
            var weekDatesTextLeaf = new TextLeaf() { LayerName = "Даты недели" };
            blob.AddChild(weekDatesTextLeaf);
            return blob;
        }
    }
}
