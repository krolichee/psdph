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
        public static string[] DayOfWeekNames => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => Localization.Localize(e)).ToArray();
        public static WeekConfig GetWeekConfig()
        {
            return new WeekConfig()
            {
                DateParameterName = "Число",
                DayDateFormat = new NoZeroDateFormat(),
                DowFormat = new ShortDowFormat().Lower,
                //DowPlaceholderLayernameList = DowLayernamePairs.ToList(),
                DowParameterName = "День недели",
                WeekDatesParameterName = "Даты недели",
                PrototypeLayerName = "Прототип дня"
            };
        }
        public static RootBlob GetWeekBlob()
        {
            var blob = new RootBlob();
            var dayBlob = new LayerBlob("Прототип дня");
            
            var dayPrototype = new PrototypeBlob() { RelativeLayerName = "Пн", LayerName = "Прототип дня" };
            dayBlob.ParameterSet.Add(new StringParameter() { Name = "Число" });
            dayBlob.ParameterSet.Add(new StringParameter() { Name = "День недели" });
            blob.AddChild(dayBlob);
            blob.AddChild(dayPrototype);
            foreach (var dow in DayOfWeekNames)
                blob.AddChild(new PlaceholderLeaf() { PrototypeBlob = dayPrototype, LayerName = dow });
            var weekDatesParameter = new StringParameter() { Name = "Даты недели" };
            blob.ParameterSet.Add(weekDatesParameter);
            return blob;
        }
    }
}
