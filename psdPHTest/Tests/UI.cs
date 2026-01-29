using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Runtime.InteropServices;
using psdPHTest.Tests;
using psdPH.Views.WeekView;
using System.IO;
using psdPH.Logic.Parameters;

namespace psdPHTest.Tests.UI
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class WeekViewWindowTest:WeekViewTest
    {
        [TestMethod]
        public void testWindow()
        {
            //PsdPhDirectories.SetBaseDirectory(Directory.GetCurrentDirectory());
            //PsdPhProject.MakeInstance("test").saveBlob(GetBlob());

            var weekConfig = GetWeekConfig();

            var weekBlob = GetWeekBlob();
            weekBlob.ParameterSet.Add(new FlagParameter("testFlag"));
            var dayBlob = weekConfig.GetDayPrototype(weekBlob);
            dayBlob.ParameterSet.Add(new FlagParameter("testFlag"));

            var weekListData = WeekListData.CreateWeekListData(weekConfig, weekBlob);

            var wv_w = new WeekViewWindow(weekListData);
            wv_w.ShowDialog();
        }
    }
}
