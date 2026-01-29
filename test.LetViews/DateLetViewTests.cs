using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.LetViews.Choose;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class DateLetViewTests
	{
        Let let;
        class TestObj
        {
            public DateTime Date;
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Date));
            let = new Let(config);
            var view = new DateLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
