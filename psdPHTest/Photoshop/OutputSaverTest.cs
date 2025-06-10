using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Photoshop
{
    [TestCategory(TestCatagories.PhotoshopManual)]
    [TestClass]
    public class OutputSaverTest
    {
        
        [TestMethod]
        public void testSave()
        {
            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            
            var doc =psApp.ActiveDocument;
            var dir = Directory.GetCurrentDirectory();
            var name = Path.GetFileName(dir);
            new OutputSaver(dir).Save(doc);
            var files = Directory.GetFiles(dir).Select(f=>Path.GetFileNameWithoutExtension(f)+Path.GetExtension(f)).ToArray();
            Assert.IsTrue(files.Contains($"{name}.png"));
            Assert.IsTrue(files.Contains($"{name}.psd"));
        }
    }
}
