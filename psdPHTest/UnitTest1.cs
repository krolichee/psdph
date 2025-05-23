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


namespace psdPHText.UI
{
    [TestClass]
    public class Rtb
    {
        static string s = string.Empty;
        public string m { get => Rtb.s; set => Rtb.s = value; }
        [TestMethod]
        public void TestM()
        {
            ParameterConfig config = new ParameterConfig(this, nameof(this.m), "Строка");
            Parameter[] parameters = new Parameter[] { Parameter.RichStringInput(config) };
            while (new ParametersInputWindow(parameters).ShowDialog()==true) ;
        }
        [TestMethod]
        public void AligmentContolUITest()
        {
            var window = new Window();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            var aliControl = new AligmentControl(30);
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
            var aliControl = new AligmentControl();
            aliControl.HorizontalAlignment=HorizontalAlignment.Stretch;
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

namespace psdPHText.Ps
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
        public void OpenImage()
        {
            psApp.Open("D:/Downloads/image.png");
        }

        [TestInitialize]
        public void Init()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            psApp = Activator.CreateInstance(psType) as Application;
            doc.ResetHistory();
        }

        [TestMethod]
        public void DynamicCast()
        {
            var _ = psApp as Document;
            Console.WriteLine((_.ActiveLayer as ArtLayer).Name);
        }
        [TestMethod]
        public void FitWithEqualizeDemo()
        { 
            int count = 4;
            for (int i = 1; i <= count; i++)
            {
                ArtLayerWr textLayer = doc.GetLayerByName($"text{i}").Wrapper();
                ArtLayerWr areaLayer = doc.GetLayerByName($"area{i}").Wrapper();
                textLayer.FitWithEqualize(areaLayer);
            }
        }

        [TestMethod]
        public void EqualizeLineWidth() {
            ArtLayerWr textLayer = doc.GetLayerByName("text").Wrapper();
            textLayer.EqualizeLineWidth();
        }
        [TestMethod]
        public void GroupLayer()
        {

        }
        [TestMethod]
        public void SplitTextLayer()
        {
            ArtLayerWr textLayer = doc.GetLayerByName("text").Wrapper();
            textLayer.SplitTextLayer();
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
            Assert.AreEqual(rect.Width,0);
        }
        [TestMethod]
        public void ViewTextItem()
        {
            ArtLayer textLayer = doc.GetLayerByName("text");
            Console.WriteLine(textLayer.TextItem.GetHashCode());
        }
        [TestMethod]
        public void CheckInteropWhenTextEditing()
        {
            var _ = (psApp.ActiveDocument.ActiveLayer as ArtLayer).TextItem.Size=50;
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
            flagLeaf.Toggle = (sender as CheckBox).IsChecked==true;
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
    public class DateFormat
    {

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
