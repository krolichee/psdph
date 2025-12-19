using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;

namespace test.Nodes.Core
{
    [TestClass]
	public class NodeGraphTests
	{
		[TestMethod]
		public void AddNodeTest()
		{
			var graph = new NodeGraph();
			graph.Nodes.Add(new EmptyNode());
		}
        [TestMethod]
        public void LetLinkTest()
        {
            var graph = new NodeGraph();
            var node1 = new SingleBoolLetsNode();
            var node2 = new SingleBoolLetsNode();
            graph.LetLink(node1.Outlets[0], node2.Inlets[0]);
        }
        [TestMethod]
        public void ChainTest()
        {
            var graph = new NodeGraph();
            var node1 = new SingleBoolLetsNode();
            var node2 = new SingleBoolLetsNode();
            graph.Chain(node1.Outlets[0], node2,false);
        }
        [TestMethod]
        public void NodeLinkTest()
        {
            var graph = new NodeGraph();
            var node1 = new EmptyNode();
            var node2 = new EmptyNode();
            graph.NodeLink(node1, node2);
        }
	}
}
