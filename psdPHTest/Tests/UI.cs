using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.InteropServices;
using psdPHTest.Tests;
using psdPH.Views.WeekView;
using System.IO;
using psdPH.RuleEditor;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;

namespace psdPHTest.Tests.UI
{
    [TestClass]
    public class TemplateEditorTest
    {
        [TestMethod]
        public void testMultiTextLeaf()
        {
            var blob = Blob.PathBlob("test.psd");
            blob.AddChild(new TextLeaf() { LayerName="text1"});
            blob.AddChild(new TextLeaf() { LayerName="text2"});
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            var c_w = new MultiTextLeafCreator(doc,blob);
            c_w.ShowDialog();
            blob.AddChildren(c_w.GetResultBatch());
            blob.getChildren<TextLeaf>().First(t=>t.LayerName == "text1");
            blob.getChildren<TextLeaf>().First(t=>t.LayerName == "text2");
        }

        [TestMethod]
        public void testMultiPlaceholderLeaf()
        {
            var blob = Blob.PathBlob("test.psd");
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            try {
                new MultiPlaceholderLeafCreator(doc, blob); 
                Assert.Fail(); 
            } catch {  }
            blob.AddChild(new PrototypeLeaf() { LayerName = "prototype" });
            
            var c_w = new MultiPlaceholderLeafCreator(doc,blob);
            c_w.ShowDialog();
            blob.AddChildren(c_w.GetResultBatch());
            blob.getChildren<PlaceholderLeaf>().First(t => t.LayerName == "layer1");
            blob.getChildren<PlaceholderLeaf>().First(t => t.LayerName == "layer2");
            
        }
    }
    
    [TestClass]
    public class ParameterTest
    {
        public object[] Objects;
        public string str=string.Empty;
        [TestMethod]
        public void testMulti()
        {
            var options = new string[] { "1", "2", "3" };
            var cfg = new SetupConfig(this,nameof(Objects),"каво");
            var parameters = new Setup[] { Setup.MultiChoose(cfg, options)};
            var pi_w = new ParametersInputWindow(parameters);
            pi_w.ShowDialog();
            Assert.IsTrue(Objects[0] as string=="1");
            Assert.IsTrue(Objects[1] as string=="3");
        }
        [TestMethod]
        public void testRtb()
        {
            var cfg = new SetupConfig(this, nameof(str), "каво");
            var parameters = new Setup[] { Setup.RichStringInput(cfg) };
            while (new ParametersInputWindow(parameters).ShowDialog() == true) ;
        }

    }
    [TestClass]
    public class WeekViewWindowTest:WeekViewTest
    {
        [TestMethod]
        public void testWindow()
        {
            //PsdPhDirectories.SetBaseDirectory(Directory.GetCurrentDirectory());
            //PsdPhProject.MakeInstance("test").saveBlob(GetBlob());

            var weekConfig = GetWeekConfig();

            var weekBlob = GetBlob();
            weekBlob.AddChild(new FlagLeaf() { Name = "testFlag"});
            var dayBlob = weekConfig.GetDayBlob(weekBlob);
            dayBlob.AddChild(new FlagLeaf() { Name = "testFlag"});

            var weekListData = WeekListData.Create(weekConfig, weekBlob);

            var wv_w = new WeekViewWindow(weekListData);
            wv_w.ShowDialog();
        }
        [TestMethod]
        public void testRuleControl()
        {
            var weekConfig = GetWeekConfig();
            var weekBlob = GetBlob();

            weekBlob.AddChild(new FlagLeaf() { Name = "testFlag" });
            var dayBlob = weekConfig.GetDayBlob(weekBlob);
            dayBlob.AddChild(new AreaLeaf() { LayerName="area"});

            new RuleEditorWindow(new DayRulesetDefinition(dayBlob)).ShowDialog();
        }
    }
    [TestClass]
    public class MiscTest
    {
        static string s = string.Empty;
        public string m { get => MiscTest.s; set => MiscTest.s = value; }
        [TestMethod]
        public void ParameterWindowTest()
        {
            SetupConfig config = new SetupConfig(this, nameof(this.m), "Строка");
            Setup[] parameters = new Setup[] { Setup.RichStringInput(config) };
            while (new ParametersInputWindow(parameters).ShowDialog() == true) ;
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
