﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Photoshop;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using psdPH.Logic;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows;
using Application = Photoshop.Application;


namespace psdPHTest.Tests.Ps
{
    [TestCategory(TestCatagories.PhotoshopManual)]
    [TestClass]
    public class ApplicationConnectionTest
    {
        [TestMethod]
        public void testConnect()
        {
            var psAppCom__ = Marshal.GetActiveObject("Photoshop.Application");
            if (psAppCom__ == null)
            {
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                psAppCom__ = Activator.CreateInstance(psType);

            }
            Assert.IsNotNull(psAppCom__);
            return;
        }
        [TestMethod]
        public void testConnectAndCast()
        {
            Application psApp;
            var psAppCom__ = Marshal.GetActiveObject("Photoshop.Application");
            psApp = psAppCom__ as Application;
            if (psApp == null)
            {
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                psAppCom__ = Activator.CreateInstance(psType);
                psApp = psAppCom__ as Application;
            }
            Assert.IsNotNull(psApp);
        }
    }
    [TestCategory(TestCatagories.ManualUI)]
    [TestClass]
    public class TextLayerTest
    {
        static Application psApp;
        Document doc => psApp.ActiveDocument;
        [TestInitialize]
        public void Init()
        {
            ArtLayer a;
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            psApp = Activator.CreateInstance(psType) as Application;
        }
        [TestMethod]
        public void testTextAndNameMatch()
        { var layer = doc.ActiveLayer as ArtLayer;
            var first_name = layer.Name;
            var layer_text = layer.TextItem.Contents;
            var text = "333";
            layer.Name = first_name;
            layer.TextItem.Contents = text;
            Assert.IsTrue(first_name != text);
        }
        [TestMethod]
        public void testFix()
        {
            doc.FixTextLayersNames();
        }
        [TestMethod]
        public void testApplyStyle()
        {
            psApp.DisplayDialogs = PsDialogModes.psDisplayNoDialogs;
            (doc.ActiveLayer as ArtLayer).ApplyStyle("sdasd");
        }
        [TestMethod]
        public void testChtoto()
        {
            ArtLayer a = doc.ActiveLayer as ArtLayer;
            a.Chtoto();
        }
    }
    [TestCategory(TestCatagories.PhotoshopManual)]
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
            //doc.ResetHistory();
        }
        [TestMethod]
        public void testSaved()
        {
            Assert.IsTrue(doc.Saved);
        }
        [TestMethod]
        public void testNotLastHistory()
        {
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
    namespace Duration
    {
        [TestCategory(TestCatagories.PhotoshopManual)]
        [TestClass]
        public class DurationTest
        {
            static Application psApp;
            Document doc => psApp.ActiveDocument;
            LayerWr layerWr => (doc.ArtLayers.Cast<ArtLayer>().ToList())[0].Wrapper();
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

    namespace Adjust
    {
        [TestCategory(TestCatagories.ManualUI)]
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
                    textLayer.FitWithEqualize(areaLayer, PhotoshopLayerExtension.AlignOptions.Default);
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
                    textLayer.FitWithEqualize(areaLayer, PhotoshopLayerExtension.AlignOptions.Default);
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

