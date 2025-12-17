using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;

namespace test.Nodes.Core
{
    [TestClass]
	public class NodeGraphTest
	{
		[TestMethod]
		public void AddNodeTest()
		{
			var graph = new NodeGraph();
			graph.Nodes.Add(new EmptyNode());
		}
        [TestMethod]
        public void LinkTest()
        {
            var graph = new NodeGraph();
            var node1 = new SingleBoolLetsNode();
            var node2 = new SingleBoolLetsNode();
            graph.Link(node1.Outlets[0], node2.Inlets[0]);
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
        public void SetRootTest()
        {
            var graph = new NodeGraph();
            graph.RootNode = new EmptyNode();
        }
	}
}
