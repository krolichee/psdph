using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH;
using System;
using System.Windows.Controls;
using System.Windows;
using psdPH.Utils.Setups;
using psdPH.Setups;

namespace psdPHTest.Tests.UI
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class MiscTest
    {
        static string s = string.Empty;
        public string m { get => MiscTest.s; set => MiscTest.s = value; }
        [TestMethod]
        public void ParameterWindowTest()
        {
            ReflectionConfig config = new ReflectionConfig(this, nameof(this.m), "Строка");
            Setup[] parameters = new Setup[] { new RichStringInputSetup(config) };
            while (new SetupsInputWindow(parameters).ShowDialog() == true) ;
        }
        [TestMethod]
        public void CalendarTest()
        {
            var window = new Window();
            var calendar = new Calendar();
            window.Content = calendar;
            calendar.BlackoutDates.Add(new CalendarDateRange(new DateTime(2025, 05, 1)));
            window.ShowDialog();
            Assert.IsTrue(calendar.SelectedDate == new DateTime(2025, 05, 1));
        }
        [TestMethod]
        public void testDatePicker()
        {
            var window = new Window();
            var calendar = new DatePicker();
            window.Content = calendar;
            calendar.BlackoutDates.Add(new CalendarDateRange(new DateTime(2025, 05, 1)));
            window.ShowDialog();
            Assert.IsTrue(calendar.SelectedDate == new DateTime(2025, 05, 1));
        }
        [TestMethod]
        public void AligmentContolUITest()
        {
            var window = new Window();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            var aliControl = new AlignmentControl(30);
            aliControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            aliControl.VerticalAlignment = VerticalAlignment.Stretch;
            aliControl.VerticalContentAlignment = VerticalAlignment.Stretch;
            aliControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            window.Content = aliControl;
            window.ShowDialog();
        }
    }
}
