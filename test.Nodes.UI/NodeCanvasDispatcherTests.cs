using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;
using psdPH.Nodes.Basic;
using psdPH.Nodes.UI;

namespace test.Nodes.UI
{
	[TestClass]
	public class NodeCanvasDispatcherTests
	{
		[TestMethod]
		public void DeleteNodeTest()
		{
			var graph = new NodeGraph();
			var dispatcher = new NodeCanvasDispatcher(graph);
			var node = new TernarNode();

            //Arrange
            graph.Nodes.Add(node);

			//Act & Assert
			dispatcher.DeleteNode(node);
            Assert.IsFalse(graph.Nodes.Contains(node));

        }
        [TestMethod]
        public void DeleteLinkTest()
        {
            var graph = new NodeGraph();
            var dispatcher = new NodeCanvasDispatcher(graph);
            var node1 = new TernarNode();
            var node2 = new TernarNode();
            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);

            //Arrange
            graph.LetLink(node1.Flowlet, node2.Flowlet);

            //Act & Assert
            var link = new NodeLetLink(NodeLet.Get(node1.Flowlet), NodeLet.Get(node2.Flowlet));
            dispatcher.DeleteLink(link);
            Assert.IsFalse(graph.IsLinked(node1,node2));

        }
        [TestMethod]
        public void PullLinkTest()
        {
            var graph = new NodeGraph();
            var dispatcher = new NodeCanvasDispatcher(graph);
            var node1 = new TernarNode();
            var node2 = new TernarNode();
            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);

            //Arrange
            dispatcher.SourceLet = NodeLet.Get(node1.OutputLet);
            dispatcher.PullLinkTo(NodeLet.Get(node2.FactorLet));

            //Act & Assert
            var link = new NodeLetLink(NodeLet.Get(node1.Flowlet), NodeLet.Get(node2.Flowlet));
            dispatcher.DeleteLink(link);
            Assert.IsTrue(graph.IsLinked(node1, node2));
        }
    }
}
