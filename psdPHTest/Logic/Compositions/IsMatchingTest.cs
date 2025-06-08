using Microsoft.VisualStudio.TestTools.UnitTesting;
using Photoshop;
using psdPH;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Logic.Compositions
{
 [TestClass]  public class IsMatchingTest
    {
        static Application psApp;
        Document doc => psApp.ActiveDocument;
        [TestInitialize]
        public void Init()
        {
            ArtLayer a;
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            psApp = Activator.CreateInstance(psType) as Application;
            Assert.IsNotNull(psApp);
        }
        [TestMethod]
        public void MyTestMethod()
        {
            string tempFilePath = Path.GetTempFileName() + ".psd"; // или с нужным расширением

            // Запись ресурса во временный файл
            File.WriteAllBytes(tempFilePath, Properties.TestResources.IsMatchingTest);
            var doc = psApp.Open(tempFilePath);
            var areaLD = LayerDescriptor.Layer("area");
            var blobLD = LayerDescriptor.Layer("smartObject");
            //var flagLD;
            var groupLD = LayerDescriptor.Group("group");
            //var imageLD = LayerDescriptor.Layer("image",PsLayerKind.psSmartObjectLayer);
            var layerLd = LayerDescriptor.Layer("shape");
            var phLD = LayerDescriptor.Layer("ph");
            //proto
            var textLD = LayerDescriptor.Layer("text",PsLayerKind.psTextLayer);

            var mainBlob = Blob.PathBlob(tempFilePath);

            mainBlob.AddChild(new AreaLeaf() { LayerName = areaLD.LayerName });
            var subBlob = Blob.LayerBlob(blobLD.LayerName);
            var subBlobLayer = new LayerLeaf() { LayerName = "shape1" };
            subBlob.AddChild(subBlobLayer);
            mainBlob.AddChild(subBlob);
            mainBlob.AddChild(new GroupLeaf() { LayerName = groupLD.LayerName });
            mainBlob.AddChild(new LayerLeaf() { LayerName = layerLd.LayerName });
            var prototype = new PrototypeLeaf(subBlob, phLD.LayerName);
            mainBlob.AddChild(prototype);
            mainBlob.AddChild(new PlaceholderLeaf() { LayerName = phLD.LayerName,Prototype = prototype});
            mainBlob.AddChild(new TextLeaf() { LayerName = textLD.LayerName});

            var matchResult = mainBlob.IsMatchingRouted(doc);

            Assert.IsTrue(matchResult.Match);

            subBlobLayer.LayerName = "corrupted";

            matchResult = mainBlob.IsMatchingRouted(doc);



            Assert.IsFalse(matchResult.Match);

            Assert.IsTrue(matchResult.MismatchRoute[0] == mainBlob);
            Assert.IsTrue(matchResult.MismatchRoute[1] == subBlob);
            Assert.IsTrue(matchResult.MismatchRoute[2] == subBlobLayer);

            Console.WriteLine(matchResult);

            
        }
    }
}
