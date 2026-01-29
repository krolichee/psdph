using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.SpecialControls;

namespace test.SpecialControls
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var window = new Window() { Content = new AlignmentControl() { Dimension = 40 } };
            window.ShowDialog();
        }
    }
}
