using Microsoft.VisualStudio.TestTools.UnitTesting;
using Photoshop;
using psdPH;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows;
using static psdPH.Logic.PhotoshopDocumentExtension;
using Application = Photoshop.Application;

namespace psdPHText.Ps
{
    [TestClass]
    public class SomeTest
    {
        Application psApp;
        Document doc => psApp.ActiveDocument;
        [TestInitialize]
        public void Init()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            psApp = Activator.CreateInstance(psType) as Application;
        }
        [TestMethod]
        [Timeout(5000)]
        public void Fit()
        {
            ArtLayer textLayer = doc.GetLayerByName("text");
            ArtLayer zoneLayer = doc.GetLayerByName("zone");
            doc.FitTextLayer(textLayer, zoneLayer);
            doc.AlignLayer(zoneLayer, textLayer, new Alignment(HorizontalAlignment.Right, VerticalAlignment.Bottom));
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void ViewTextItem()
        {
            ArtLayer textLayer = doc.GetLayerByName("text");
            Console.WriteLine(textLayer.TextItem.GetHashCode());
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
