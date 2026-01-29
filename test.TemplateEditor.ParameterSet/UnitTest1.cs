using System;
using System.IO;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.CED;
using psdPH.Localization;
using psdPH.Parameters;
using psdPH.TemplateEditor.Parameters;

namespace test.TemplateEditor.Parameters
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LocalizationService.InitializeLocalizations();
            var parSet = new ParameterSet();
            var handler = new ParameterHandler(parSet);
            var window = new Window();
            window.Content = CEDStackUI.CreateCEDStack(handler);
            window.ShowDialog();
        }
    }
}
