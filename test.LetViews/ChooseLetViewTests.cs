using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.LetViews.Check;
using psdPH.LetViews.Choose;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class ChooseLetViewTests
	{
        Let let;
        class TestObj
        {
            public int Selected { get; set; }
            public int[] Options => new int[] { 1, 2, 3, 4 };
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Selected));
            let = new Let(config);
            obj.Selected = 5;
            var view = new ChooseLetView(let, obj.Options.Cast<object>().ToArray());
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
