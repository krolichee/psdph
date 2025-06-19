using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPHTest.Logic
{
    namespace Parameters
    {
    [TestCategory(TestCatagories.PhotoshopTestDocs)]
    [TestClass]
    public class ParameterTest
    {
        [TestMethod]
        public void compileTest()
        {
            var blob = new RootBlob();
            var parameter = new StringParameter() { Name = "надпись" };
            var textLeaf = new TextLeaf() { LayerName = "text" };
            blob.AddChild(textLeaf);
            blob.ParameterSet.Add(parameter);
            string tempFilePath = Path.GetTempFileName() + ".psd"; // или с нужным расширением
                File.WriteAllBytes(tempFilePath, Properties.TestResources.Basic);
            var doc = PhotoshopWrapper.OpenDocument(tempFilePath);
            parameter.Value = "hzzzz";
            doc.Rollback();
            blob.Apply(doc);
            var result = doc.GetLayerByName(textLeaf.LayerName).TextItem.Contents;
            Assert.IsTrue(result == parameter.Text);
        }
    }
}
}
