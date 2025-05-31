using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Windows.Controls;
using System.Windows;
using Condition = psdPH.Logic.Rules.Condition;
using HAli = System.Windows.HorizontalAlignment;
using VAli = System.Windows.VerticalAlignment;
using psdPH.Views.WeekView;
using psdPHTest.Tests;
using psdPH.Utils;

namespace psdPHTest.Automatic
{
    [TestClass]
    public class XMLSerializationTest
    {
        [TestMethod]
        public void testSave()
        {
           var blob= Blob.PathBlob("очень сильно заболел хуй");
            Assert.IsTrue(DiskOperations.SaveXml("test.xml", blob).Serialized);
        }
        [TestMethod]
        public void testOpen()
        {
            DiskOperations.OpenXml<Blob>("test.xml");
        }
        [TestMethod]
        public void testKnownTypes()
        {
            var blob = Blob.PathBlob("test.xml");
            var cond = new DummyCondition(null);
            blob.RuleSet.AddRule(new AlignRule(blob) { Alignment = Alignment.Create("up","left"),AreaLayerName = "2",LayerName = "2"});
            Assert.IsTrue(DiskOperations.SaveXml("test.xml", blob).Serialized);
        }
    }
    [TestClass]
    public class AligmentRuleTest
    {
        Blob MainBlob;
        FlagLeaf flagLeaf;
        void _(object sender, RoutedEventArgs e)
        {
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            flagLeaf.Toggle = (sender as CheckBox).IsChecked == true;
            MainBlob.Apply(doc);
        }
        Blob GetBlob()
        {
            var blob = Blob.PathBlob("");
            flagLeaf = new FlagLeaf("sadism") { Toggle = true };
            var on_area = new AreaLeaf() { LayerName = "on_area" };
            var off_area = new AreaLeaf() { LayerName = "off_area" };
            var layer1Leaf = new TextLeaf() { LayerName = "lorem", Text = "Lorem Ipsum" };
            var objLayer = new TextLeaf() { LayerName = "obj" };
            var controlLayer = new LayerLeaf() { LayerName = "control" };
            blob.AddChild(flagLeaf);
            blob.AddChild(on_area);
            blob.AddChild(off_area);
            blob.AddChild(layer1Leaf);
            Condition true_condition = new FlagCondition(blob) { FlagLeaf = flagLeaf, Value = true };
            Condition false_condition = new FlagCondition(blob) { FlagLeaf = flagLeaf, Value = false };
            blob.RuleSet.Rules.Add(
                new AlignRule(blob)
                {
                    LayerComposition = layer1Leaf,
                    AreaLeaf = on_area,
                    Alignment = Alignment.Create("center", "left"),
                    Condition = true_condition
                }
                );
            blob.RuleSet.Rules.Add(
                new AlignRule(blob)
                {
                    LayerComposition = layer1Leaf,
                    AreaLeaf = off_area,
                    Alignment = Alignment.Create("center", "left"),
                    Condition = false_condition
                }
                );
            blob.RuleSet.Rules.Add(
                new VisibleRule(blob) { LayerComposition = objLayer, Condition = true_condition }
                );
            return blob;
        }
        [TestInitialize]
        public void Init()
        {
            MainBlob = GetBlob();
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            var window = new Window();
            var chb = new CheckBox();
            window.Content = chb;
            chb.HorizontalAlignment = HAli.Center;
            chb.VerticalAlignment = VAli.Center;
            chb.Click += _;
            window.Height = window.MinHeight = 100;
            window.Width = window.MinWidth = 70;
            window.WindowStyle = WindowStyle.ToolWindow;
            window.ShowDialog();
        }
        [TestMethod]
        public void AlignRuleTest()
        {

        }
    }

    }
    
    namespace WeekRules
    {
        [TestClass]
        public class WeekRules: WeekViewTest
        {
            [TestMethod]
            public void FromBlobTest()
            {
                Blob blob = GetBlob();
                var dayBlob = DowBlob.FromBlob(blob, 0, DayOfWeek.Monday);
            }

            EveryNDayCondition GetEveryNDayCondition(int interval, DateTime startDateTime, DowBlob blob)
            {
                return new EveryNDayCondition(blob) { Interval = interval, StartDateTime = startDateTime };
            }
            [DataTestMethod]
            [DataRow(2, DayOfWeek.Monday, true)]
            [DataRow(3, DayOfWeek.Tuesday, false)]
            [DataRow(2, DayOfWeek.Wednesday, true)]
            [DataRow(3, DayOfWeek.Thursday, true)]
            public void EveryNDayConditionTest(int interval, DayOfWeek dayOfWeek, bool result)
            {
                var blob = GetBlob();
                var weekConfig = GetWeekConfig();

                var weekListData = WeekListData.Create(weekConfig,blob);
                var currentWeek = WeekTime.CurrentWeek;

                weekListData.NewWeek(currentWeek);
                var weekData = weekListData.Weeks[0];

                var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
                var dayBlob = new DowBlob() { Dow = dayOfWeek, Week = currentWeek };
                var everynCondition = GetEveryNDayCondition(interval, startDateTime, dayBlob);

                Assert.IsTrue(everynCondition.IsValid() == result);

            }
            [DataTestMethod]
            [DataRow(2, DayOfWeek.Monday, true)]
            [DataRow(3, DayOfWeek.Tuesday, false)]
            [DataRow(2, DayOfWeek.Wednesday, true)]
            [DataRow(3, DayOfWeek.Thursday, true)]
            public void WeekRulesTest(int interval, DayOfWeek dayOfWeek, bool result)
            {
                var blob = GetBlob();
                var weekConfig = GetWeekConfig();

                weekConfig.GetDayPrototype(blob).Blob.AddChild(new FlagLeaf("uvu"));

                var weekListData = WeekListData.Create(weekConfig,blob);
                var currentWeek = WeekTime.CurrentWeek;

                weekListData.NewWeek(currentWeek);
                var weekData = weekListData.Weeks[0];

                var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
                var everynCondition = new EveryNDayCondition() { Interval = interval, StartDateTime = startDateTime };
                var everynRule = new FlagRule() { FlagName = "uvu", Condition = everynCondition };

                weekListData.DayRules.AddRule(everynRule);
                var mainBlob = weekData.Prepare();

                var dayPh = mainBlob.getChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == dayOfWeek);
                var dayBlob = dayPh.Replacement as DowBlob;

                Assert.IsTrue(dayBlob.getChildren<FlagLeaf>().First(f => f.Name == "uvu").Toggle == result);
            }
            [TestMethod]
            public void testInjectRules()
            {
                var blob = GetBlob();
                var weekConfig = GetWeekConfig();
                weekConfig.GetDayPrototype(blob).Blob.AddChild(new FlagLeaf("test1"));

                blob.AddChild(new FlagLeaf("test"));

                var weekListData = WeekListData.Create(weekConfig, blob);
#pragma warning disable CS0612 // Тип или член устарел
                var weekCondition = new WeekCondition(blob) { Week = WeekTime.CurrentWeek };
#pragma warning restore CS0612 // Тип или член устарел
                var weekFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test" };

                var ndayCondition = new EveryNDayCondition(blob) { Interval = 2,StartDateTime=WeekTime.GetDateByWeekAndDay(WeekTime.CurrentWeek,DayOfWeek.Monday)};
                var dayFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test1" };


                weekListData.DayRules.AddRule(dayFlagRule);
                weekListData.WeekRules.AddRule(weekFlagRule);

                weekListData.NewWeek();

                var weekBlob = weekListData.Weeks[0].Prepare();
                weekBlob.CoreApply();

                Assert.IsTrue(weekBlob.getChildren<FlagLeaf>().First(f => f.Name == "test").Toggle);

                var mondayBlob = weekBlob.getChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == DayOfWeek.Monday).Replacement;
                Assert.IsTrue(mondayBlob.getChildren<FlagLeaf>().First(f => f.Name == "test1").Toggle);
            }
        }
    }
    [TestClass]
    public class CompositionTest
    {

        [TestMethod]
        public void CompositionChildrenObserving()
        {
            bool eventRaised = false;
            void a()
            {
                eventRaised = true;
            }
            Composition composition = new Blob();
            composition.ChildrenUpdatedEvent += a;
            composition.AddChild(new TextLeaf());
            Assert.IsTrue(eventRaised);
        }
        [TestMethod]
        public void RuleSetChildrenObserving()
        {
            bool eventRaised = false;
            void a()
            {
                eventRaised = true;
            }
            Composition composition = new Blob();
            composition.RulesetUpdatedEvent += a;
            composition.RuleSet.Rules.Add(new TextFontSizeRule());
            Assert.IsTrue(eventRaised);
        }
    }
}

