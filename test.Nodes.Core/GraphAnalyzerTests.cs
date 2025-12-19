using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;

namespace test.Nodes.Core
{
	[TestClass]
	public class GraphAnalyzerTests
	{
		[TestMethod]
#if !DEBUG
		[Timeout(50)]
#endif
        public void GetRootNode_test()
		{
			var graph = new NodeGraph();
			var node1 = new EmptyNode();
			var node2 = new EmptyNode();

			graph.Nodes.Add(node1);
			graph.Nodes.Add(node2);
			graph.NodeLink(node1, node2);

			var rootNode = GraphAnalyzer.GetRootNode(graph);
			Assert.IsTrue(rootNode == node1);

		}
		[TestMethod]
		public void IsCycledTest()
		{
            var graph = new NodeGraph();
            var node1 = new EmptyNode();
            var node2 = new EmptyNode();

            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);
            graph.NodeLink(node1, node2);

			Assert.IsFalse(GraphAnalyzer.IsCycled(graph));

            graph.NodeLink(node2, node1);
            Assert.IsTrue(GraphAnalyzer.IsCycled(graph));
        }
		[TestMethod]
		public void GetRootishNodes_test()
		{
            var graph = new NodeGraph();
            var node1 = new EmptyNode("a");
            var node2 = new EmptyNode("b");
            var node3 = new EmptyNode("c");

            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);
            graph.Nodes.Add(node3);
            graph.NodeLink(node2, node1);
            graph.NodeLink(node3, node1);

			var rootishNodes = GraphAnalyzer.GetRootishNodes(graph).ToArray(); ;
            Assert.IsTrue(rootishNodes.Count()==2);
            Assert.IsTrue(rootishNodes.Contains(node2));
            Assert.IsTrue(rootishNodes.Contains(node3));
        }
	}
}
