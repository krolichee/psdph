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
        public void LayerCast()
        {
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
            doc.LayerSets.Add();
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
