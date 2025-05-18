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

namespace psdPHText.Ps
{
    [TestClass]
    public class FitRuleTest
    {
        
    }
    public static class LayerExtension
    {
        
        

        public static void AdjustLayerSetTo(this LayerSet layer, ArtLayer areaLayer)
        {
            double layerRatio = layer.GetBoundRect().Width / layer.GetBoundRect().Height;
            double areaRatio = areaLayer.GetBoundRect().Width / areaLayer.GetBoundRect().Height;
            double resizeRatio;
            void adjustByWidth()
            {
                resizeRatio = areaLayer.GetBoundRect().Width/ layer.GetBoundRect().Width*100;
            }
            void adjustByHeight()
            {
                resizeRatio = areaLayer.GetBoundRect().Height / layer.GetBoundRect().Height * 100;
            }
            if (layerRatio >= areaRatio)
                adjustByWidth();
            else
                adjustByHeight();
            layer.Resize(resizeRatio, resizeRatio);
        }


        public static LayerSet EqualizeLineWidth(this ArtLayer textLayer)
        {
            LayerSet lineLayerSet = textLayer.SplitTextLayer();
            ArtLayer[] lineLayers = lineLayerSet.ArtLayers.Cast<ArtLayer>().ToArray();
            double maxWidth = lineLayers.Max((l) => l.GetBoundRect().Width);

            List<double> prevLineGaps = new List<double> { 0 };
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                ArtLayer layer = lineLayers[i];
                ArtLayer prevLayer = lineLayers[i - 1];
                prevLineGaps.Add(layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom);
            }
            lineLayers[0].AdjustTextLayerByWidth(maxWidth);
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                double prevLineGap = prevLineGaps[i];
                ArtLayer layer = lineLayers[i];
                ArtLayer prevLayer = lineLayers[i - 1];
                layer.AdjustTextLayerByWidth(maxWidth);
                double curGap = layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom;
                layer.TranslateV(new Vector(0, prevLineGap - curGap));
            }
            return lineLayerSet;
        }
        public static void FitWithEqualize(this ArtLayer textLayer,ArtLayer areaLayer) {
            LayerSet equalized = textLayer.EqualizeLineWidth();
            equalized.AdjustLayerSetTo(areaLayer);
            equalized.AlignLayer(areaLayer, new Alignment(HorizontalAlignment.Center, VerticalAlignment.Center));
            equalized.OnStyle();
        }

    }
    [TestClass]
    public class ManualPhotoshopTests
    {
        static Application psApp;
        Document doc => psApp.ActiveDocument;
        
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
                ArtLayer textLayer = doc.GetLayerByName($"text{i}");
                ArtLayer areaLayer = doc.GetLayerByName($"area{i}");
                textLayer.FitWithEqualize(areaLayer);
            }
        }

        [TestMethod]
        public void EqualizeLineWidth() {
            ArtLayer textLayer = doc.GetLayerByName("text");
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
            ArtLayer textLayer = doc.GetLayerByName("text");
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
            ArtLayer textLayer = doc.GetLayerByName("empty_text");
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
