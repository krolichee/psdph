using Microsoft.VisualStudio.TestTools.UnitTesting;
using Photoshop;
using psdPH;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;
using static psdPH.Logic.PhotoshopDocumentExtension;
using Application = Photoshop.Application;
using Microsoft.CSharp;
using System.Windows.Controls;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Photoshop;
using System.Windows.Controls.Primitives;
using System.Reflection;
using Condition = psdPH.Logic.Rules.Condition;
using System.Threading;
using HAli = System.Windows.HorizontalAlignment;
using VAli = System.Windows.VerticalAlignment;
using System.Threading.Tasks;
using psdPH.Views.WeekView;
using System.Xml.Serialization;
using psdPH.Views.WeekView.Logic;
using static psdPH.WeekConfig;
using psdPH.Utils;
using System.IO;
using System.Text;


namespace psdPHTest.UI
{
    [TestClass]
    public class MiscTest
    {
        static string s = string.Empty;
        public string m { get => MiscTest.s; set => MiscTest.s = value; }
        [TestMethod]
        public void ParameterWindowTest()
        {
            ParameterConfig config = new ParameterConfig(this, nameof(this.m), "Строка");
            Parameter[] parameters = new Parameter[] { Parameter.RichStringInput(config) };
            while (new ParametersInputWindow(parameters).ShowDialog() == true) ;
        }
        [TestMethod]
        public void CalendarTest()
        {
            var window = new Window();
            var calendar = new Calendar();
            window.Content = calendar;
            window.ShowDialog();
          // calendar.SelectedDate;
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
        [TestMethod]
        public void AligmentContolValuesTest()
        {
            var window = new Window();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            var aliControl = new AlignmentControl();
            aliControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            aliControl.VerticalAlignment = VerticalAlignment.Stretch;
            aliControl.VerticalContentAlignment = VerticalAlignment.Stretch;
            aliControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            window.Content = aliControl;

            Dictionary<Button, Alignment> btnAli = new Dictionary<Button, Alignment>()
            {
                { aliControl.upLeft,Alignment.Create("up","left")},
                { aliControl.upCenter,Alignment.Create("up","center")},
                { aliControl.upRight,Alignment.Create("up","right")},
                { aliControl.centerLeft,Alignment.Create("center","left")},
                { aliControl.centerCenter,Alignment.Create("center","center")},
                { aliControl.centerRight,Alignment.Create("center","right")},
                { aliControl.downLeft,Alignment.Create("down","left")},
                { aliControl.downCenter,Alignment.Create("down","center")},
                { aliControl.downRight,Alignment.Create("down","right")}
            };

            foreach (Button button in btnAli.Keys)
            {
                button.Command.Execute(button.CommandParameter);
                Assert.IsTrue(aliControl.GetResultAlignment().Equals(btnAli[button]));
            }
        }
    }
}

namespace psdPHTest.Ps
{

    public static class LayerExtension
    {

    }
    [TestClass]
    public class ManualPhotoshopTests
    {
        static Application psApp;
        Document doc => psApp.ActiveDocument;
        [TestMethod]
        

        [TestInitialize]
        public void Init()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            psApp = Activator.CreateInstance(psType) as Application;
            doc.ResetHistory();
        }
        public void OpenImage()
        {
            psApp.Open("D:/Downloads/image.png");
        }
        [TestMethod]
        public void DynamicCast()
        {
            var _ = psApp as Document;
            Console.WriteLine((_.ActiveLayer as ArtLayer).Name);
        }
        [TestMethod]
        
        public void GroupLayer()
        {

        }
        [TestMethod]
        public void SplitTextLayer()
        {
            TextLayerWr textLayerWr = new TextLayerWr(doc.GetLayerByName("text"));
            textLayerWr.SplitTextLayer();
        }
        [TestMethod]
        public void EmptyTextTextItemError()
        {
            ArtLayer textLayer = doc.GetLayerByName("empty_text");
            try
            {
                var _ = textLayer.TextItem.Width;
                Assert.IsTrue(false);
            }
            catch { Assert.IsTrue(true); }

        }
        [TestMethod]
        public void EmptyTextRectWidth()
        {
            ArtLayerWr textLayer = doc.GetLayerByName("empty_text").Wrapper();
            var rect = textLayer.GetBoundRect();
            Assert.AreEqual(rect.Width, 0);
        }
        [TestMethod]
        public void ViewTextItem()
        {
            ArtLayer textLayer = doc.GetLayerByName("text");
            Console.WriteLine(textLayer.TextItem.GetHashCode());
        }
    }
    namespace psdPHTest.Ps.Duration
    {
        [TestClass]
        public class DurationTest
        {
            static Application psApp;
            Document doc => psApp.ActiveDocument;
            LayerWr layerWr => (doc.ArtLayers.Cast<ArtLayer>().ToList()) [0].Wrapper();
            [TestInitialize]
            public void Init()
            {
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                psApp = Activator.CreateInstance(psType) as Application;
                doc.ResetHistory();
            }
            [TestMethod]
            public void OnStyle()
            {
                layerWr.OnStyle();
            }
            [TestMethod]
            public void OffStyle()
            {
                layerWr.OffStyle();
            }
        }
    }

    namespace psdPHTest.Ps.Adjust {
        [TestClass]
        public class AdjustTest
        {
            static Application psApp;
            Document _doc => psApp.ActiveDocument;
            


            [TestInitialize]
            public void Init()
            {
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                psApp = Activator.CreateInstance(psType) as Application;
                _doc.ResetHistory();
            }
            [TestMethod]
            public void FitWithEqualizeDemo()
            {
                int count = 4;
                for (int i = 1; i <= count; i++)
                {
                    TextLayerWr textLayer = _doc.GetLayerByName($"text{i}").TextWrapper();
                    ArtLayerWr areaLayer = _doc.GetLayerByName($"area{i}").Wrapper();
                    textLayer.FitWithEqualize(areaLayer, Alignment.Create("center", "center"));
                }
            }
            [TestMethod]
            public void EqualizeLineWidth()
            {
                TextLayerWr textLayer = _doc.GetLayerByName("text").TextWrapper();
                textLayer.EqualizeLineWidth();
            }

            static void DoWork(Document doc)
            {
                int count = 4;
                for (int i = 1; i <= count; i++)
                {
                    TextLayerWr textLayer = doc.GetLayerByName($"text{i}").TextWrapper();
                    ArtLayerWr areaLayer = doc.GetLayerByName($"area{i}").Wrapper();
                    textLayer.FitWithEqualize(areaLayer, Alignment.Create("center", "center"));
                    Console.WriteLine($"обработка пары {i} в документе {doc.Name}");
                }
            }
            [TestMethod]
            public void ThreadingTest()
            {
                Document[] docs = psApp.Documents.Cast<Document>().ToArray();
                int threadCount = docs.Length;
                Thread[] threads = new Thread[threadCount];

                // Создание и запуск потоков

                threads[0] = new Thread(() => DoWork(docs[0]));
                threads[0].Start();
                threads[1] = new Thread(() => DoWork(docs[1]));
                threads[1].Start();

                // Ожидание завершения всех потоков
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }
        }
    }
}
namespace psdPHTest.Automatic
{
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
            blob.addChild(flagLeaf);
            blob.addChild(on_area);
            blob.addChild(off_area);
            blob.addChild(layer1Leaf);
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
    [TestClass]
    public class SimpleView
    {
        [ClassInitialize]
        public void Initialize()
        {
            string baseDirectory = @"C:\\Users\\Puziko\\source\\repos\\psdPH\\psdPH\\testResources\\TestProjects\\";
            Directories.SetBaseDirectory(baseDirectory);
            var project = PsdPhProject.MakeInstance("simple");
        }
        [TestMethod]
        public void OpenView()
        {
        }
        [TestMethod]
        public void OpenOrCreateView()
        {
        }
        public void CreateView()
        {
        }
        [TestMethod]
        public void SaveView()
        {
        }
        [TestMethod]
        public void NewRow()
        {
        }

    }
    [TestClass]
    public class WeekViewTest
    {
        public static DowLayernamePair GetPair(DayOfWeek dow) => new DowLayernamePair(dow, dow.GetDescription());
        public static DowLayernamePair[] DowLayernamePairs => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => GetPair(e)).ToArray();
        public static string[] DayOfWeekNames => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(e => e.GetDescription()).ToArray();
        public static WeekConfig GetWeekConfig()
        {
            return new WeekConfig()
            {
                DateTextLeafLayerName = "Число",
                DayDateFormat = new NoZeroDateFormat(),
                DayDowFormat = new ShortDowFormat().Lower,
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
            dayBlob.addChild(new TextLeaf() { LayerName = "Число" });
            dayBlob.addChild(new TextLeaf() { LayerName = "День недели" });
            var dayPrototype = new PrototypeLeaf() { Blob = dayBlob, RelativeLayerName = "Пн" };
            blob.addChild(dayBlob);
            blob.addChild(dayPrototype);
            foreach (var dow in DayOfWeekNames)
                blob.addChild(new PlaceholderLeaf() { Prototype = dayPrototype, LayerName = dow });
            var weekDatesTextLeaf = new TextLeaf() { LayerName = "Даты недели" };
            blob.addChild(weekDatesTextLeaf);
            return blob;
        }
        [TestClass]
        public class DateFormatTest
        {
            [TestMethod]
            public void TestM()
            {
                WeekView.CreateWeekConfig(GetBlob());
            }
        }
        [TestClass]
        public class WeekRules
        {
            [TestMethod]
            public void FromBlobTest()
            {
                Blob blob = GetBlob();
                var dayBlob = DowBlob.FromBlob(blob,0,DayOfWeek.Monday);
            }
            
            EveryNDayCondition GetEveryNDayCondition(int interval,DateTime startDateTime, DowBlob blob)
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

                var weekListData = new WeekListData() { RootBlob = blob, WeekConfig = weekConfig };
                var currentWeek = WeekTime.CurrentWeek;

                weekListData.NewWeek(currentWeek);
                var weekData = weekListData.Weeks[0];

                var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
                var dayBlob =  new DowBlob() { Dow = dayOfWeek, Week = currentWeek };
                var everynCondition = GetEveryNDayCondition(interval,startDateTime,dayBlob);

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

                weekConfig.GetDayPrototype(blob).Blob.addChild(new FlagLeaf("uvu"));

                var weekListData = new WeekListData() { RootBlob = blob, WeekConfig = weekConfig };
                var currentWeek = WeekTime.CurrentWeek;

                weekListData.NewWeek(currentWeek);
                var weekData = weekListData.Weeks[0];

                var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
                var everynCondition = new EveryNDayCondition() { Interval = interval, StartDateTime = startDateTime };
                var everynRule = new FlagRule() { FlagName = "uvu", Condition = everynCondition };

                weekConfig.DayRules.Add(everynRule);
                var mainBlob = weekData.Prepare();

                var dayPh = mainBlob.getChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == dayOfWeek);
                var dayBlob = dayPh.Replacement as DowBlob;
                
                Assert.IsTrue(dayBlob.getChildren<FlagLeaf>().First(f => f.Name == "uvu").Toggle == result);
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
            composition.addChild(new TextLeaf());
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
    [TestClass]
    internal class ConcreteRuleTest
    {
        string testDirectory = "C:\\Users\\Puziko\\source\\repos\\psdPHTest\\psdPHTest";
        [TestMethod]
        public void Fitting()
        {
            Directories.BaseDirectory = testDirectory;
            Blob blob = PsdPhProject.openOrCreateMainBlob("Fitting");
            blob.addChild(new TextLeaf());
        }

    }
}
namespace psdPHTest.Manual
{

}
