using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Parameters;
using psdPH.Nodes;
using psdPH.Nodes.UI;

namespace psdPHTest.Nodes.UI
{
	[TestCategory(TestCategories.ManualUI)]
	[TestClass]
	public class NodeUITest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var window = new Window();
			var canvas = new Canvas();
			var fpar = new FlagParameter();
			var node = new MuxNode();
			var nodeUI = new NodeUI(node);
			window.Content = canvas;
			canvas.Background = new SolidColorBrush(Colors.AliceBlue);
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            nodeUI.HorizontalAlignment = HorizontalAlignment.Center;
			nodeUI.VerticalAlignment = VerticalAlignment.Center;
			Canvas.SetLeft(nodeUI, 100);
			Canvas.SetTop(nodeUI, 100);
			canvas.Children.Add(nodeUI);
			window.ShowDialog();
		}
	}
}
