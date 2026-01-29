using System;
using System.IO;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.CED;
using psdPH.Context;
using psdPH.Localization;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.TemplateEditor.Structure;
using psdPHTest;

namespace test.TemplateEditor.Structure
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LocalizationService.InitializeLocalizations();
            var blob = new RootBlob();
            var path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
            path = Path.Combine(path,"test.psd");
            var doc = PhotoshopWrapper.Instance.OpenDocumentWr(path);
            var context = new PsdPhContext(doc,blob);
            var handler = new StructureStackHandler(context);
            var window = new Window();
            window.Content = CEDStackUI.CreateCEDStack(handler);
            window.ShowDialog();
            
        }
    }
}
