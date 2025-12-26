using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.Photoshop;
using psdPH.Setups;

namespace test.LetViews
{
    [TestClass]
    public class UnitTest1
    {
        class TestObj
        {
            public Alignment Alignment { get; set; }
        }
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Alignment));
            var let = new Let(config);
            var view = new AlignmentLetView(let);
            var window = new Window() { Content = view.Control };
            window.ShowDialog();
        }
    }
}
