using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Views.WeekView.Logic;

namespace psdPHTest.Views
{
	[TestClass]
	public class ExcelExportTest:ProjectTestSuite
	{
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
        public static DowLayernamePair GetPair(DayOfWeek dow) => new DowLayernamePair(dow, Localization.LocalizeObj(dow));
        public static DowLayernamePair[] DowLayernamePairs => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => GetPair(e)).ToArray();
        public static string[] DayOfWeekNames => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => Localization.LocalizeObj(e)).ToArray();
        public static Blob GetWeekBlob()
        {
            var blob = Blob.PathBlob("");
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
        [TestMethod]
		public void testExport()
		{
            var blob = GetWeekBlob();
            var weekConfig = GetWeekConfig();
            var weekListData = WeekListData.CreateWeekListData(weekConfig,blob);

            var exporter = new WeekViewExcelExporter(weekListData);
            exporter.Export();
            var projectDir = PsdPhDirectories.ProjectDirectory(ProjectName);
            

        }
        [TestMethod]
        public void testImport()
        {

        }
    }

    public class WeekViewExcelExporter
    {
        private WeekListData weekListData;

        public WeekViewExcelExporter(WeekListData weekListData)
        {
            this.weekListData = weekListData;
        }
        
        public void Export()
        {
            throw new NotImplementedException();
        }
    }
}
