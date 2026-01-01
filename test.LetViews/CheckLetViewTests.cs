using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.LetViews.Check;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class CheckLetViewTests
	{
        Let let;
        class TestObj
        {
            public bool Bool { get; set; }
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Bool));
            let = new Let(config);
            let.Value = true;
            var view = new CheckLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
