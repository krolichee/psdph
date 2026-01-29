using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class EnumLetViewTests
	{
        Let let;
        class TestObj
        {
            public ExecutionScope Scope;
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Scope));
            let = new Let(config);
            var view = new EnumLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
