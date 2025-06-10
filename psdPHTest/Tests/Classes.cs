using psdPH.Logic.Compositions;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using psdPH.Logic.Parameters;

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
                DateParameterName = "Число",
                DayDateFormat = new NoZeroDateFormat(),
                DowFormat = new ShortDowFormat().Lower,
                DowPlaceholderLayernameList = DowLayernamePairs.ToList(),
                DowParameterName = "День недели",
                WeekDatesParameterName = "Даты недели",
                PrototypeLayerName = "Прототип дня"
            };
        }
        public static Blob GetWeekBlob()
        {
            var blob = Blob.PathBlob("C:\\ProgramData\\psdPH\\Projects\\№пример\\template.psd");
            var dayBlob = Blob.LayerBlob("Прототип дня");
            dayBlob.ParameterSet.Add(new StringParameter() { Name = "Число" });
            dayBlob.ParameterSet.Add(new StringParameter() { Name = "День недели" });
            var dayPrototype = new PrototypeLeaf() { Blob = dayBlob, RelativeLayerName = "Пн", LayerName = "Прототип дня" };
            blob.AddChild(dayBlob);
            blob.AddChild(dayPrototype);
            foreach (var dow in DayOfWeekNames)
                blob.AddChild(new PlaceholderLeaf() { Prototype = dayPrototype, LayerName = dow });
            var weekDatesParameter = new StringParameter() { Name = "Даты недели" };
            blob.ParameterSet.Add(weekDatesParameter);
            return blob;
        }
    }
}
