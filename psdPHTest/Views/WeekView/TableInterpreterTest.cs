using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Views.WeekView.Windows;
using psdPHTest.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Views.WeekView
{
    [TestClass]
    public class TableInterpreterTest: WeekViewTest
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var blob = GetWeekBlob();
            var weekConfig = GetWeekConfig();
            var weekListData = WeekListData.Create(weekConfig, blob);
            weekListData.NewWeek();

            var wvi_w = new WeekViewTableInputIntepreter(weekListData.Weeks[0]);
            wvi_w.ShowDialog();
        }
    }
}
