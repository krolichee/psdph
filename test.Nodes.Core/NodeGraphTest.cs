using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;

namespace test.Nodes.Core
{
    class EmptyNode : Node
    {
        public override Let[] Inlets => new Let[0];

        public override Let[] Outlets => new Let[0];

        public override Let[] Chain => new Let[0];

        public override void Execute(psdPH.Photoshop.DocumentWr doc)
        {
            throw new NotImplementedException();
        }
    }
    class SingleBoolLetsNode : EmptyNode
    {
        public override Let[] Outlets => new Let[] {
            new Let(null,"kavabanga",typeof(bool))
        };
        public override Let[] Inlets => new Let[] {
            new Let(null,"onobanga",typeof(bool))
        };
    }
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
            graph.Chain(node1.Chain[0], node2,false);
        }
        [TestMethod]
        public void SetRootTest()
        {
            var graph = new NodeGraph();
            graph.RootNode = new EmptyNode();
        }
	}
}
