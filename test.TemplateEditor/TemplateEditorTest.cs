using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Photoshop;

namespace psdPHTest.Tests.UI
{
    [TestCategory(TestCategories.PhotoshopManual)]
    [TestClass]
    public class TemplateEditorTest
    {
        [TestMethod]
        public void testMultiTextLeaf()
        {
            var blob = new RootBlob() ;
            blob.AddChild(new TextLeaf() { LayerName="text1"});
            blob.AddChild(new TextLeaf() { LayerName="text2"});
            var doc = PhotoshopWrapper.GetActiveDocument();
            var c_w = new MultiTextLeafCreator(doc,blob);
            c_w.ShowDialog();
            blob.AddChildren(c_w.GetResultBatch());
            blob.GetChildren<TextLeaf>().First(t=>t.LayerName == "text1");
            blob.GetChildren<TextLeaf>().First(t=>t.LayerName == "text2");
        }

        [TestMethod]
        public void testMultiPlaceholderLeaf()
        {
            var blob = new RootBlob();
            var doc = PhotoshopWrapper.GetActiveDocument();//.GetPhotoshopApplication().ActiveDocument;
            try {
                new MultiPlaceholderLeafCreator(doc, blob); 
                Assert.Fail(); 
            } catch {  }
            blob.AddChild(new PrototypeBlob() { LayerName = "prototype" });
            
            var c_w = new MultiPlaceholderLeafCreator(doc,blob);
            c_w.ShowDialog();
            blob.AddChildren(c_w.GetResultBatch());
            blob.GetChildren<PlaceholderLeaf>().First(t => t.LayerName == "layer1");
            blob.GetChildren<PlaceholderLeaf>().First(t => t.LayerName == "layer2");
            
        }
    }
}
