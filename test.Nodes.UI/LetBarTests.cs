using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.LetViews.Check;
using psdPH.Nodes.UI;
using psdPHTest;

namespace test.Nodes.UI
{
	[TestClass]
	public class LetBarTests
	{
		class TestObj
		{
			public bool B;
		}
		[TestCategory(TestCategories.ManualUI)]
		[TestMethod]
		public void TestMethod1()
		{
			var obj = new TestObj();
			var let = Let.FromField(obj, nameof(obj.B));
			var letView = new CheckLetView(let);
			
			var w = new Window();
			var letBar = new LetBar(letView);
			w.Content = letBar;
			w.ShowDialog();
		}
	}
}
