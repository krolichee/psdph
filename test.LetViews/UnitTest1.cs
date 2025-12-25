using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.LetViews;

namespace test.LetViews
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var view = new AlignmentLetViewControl("ASS");
            var window = new Window() { Content = view };
            window.ShowDialog();
        }
    }
}
