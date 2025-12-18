using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;

namespace test.Nodes.Core
{
	[TestClass]
	public class NodeLetLinkTests
	{
		private class Single
		{
			int a;
			public Let Let => new Let(this, "122", typeof(int), () => a, (v) => a = (int)v);
		}
		[TestMethod]
		public void TestPush()
		{
			//Arrange
			var node1 = new SingleBoolLetsNode();
			var node2 = new SingleBoolLetsNode();
			var link = new NodeLetLink(new NodeLet(node1,node1.Outlets[0]), new NodeLet(node2, node2.Inlets[0]));
			//Act&Assert
			node1.LetBool = true;
			link.Push();
			Assert.IsTrue(node2.LetBool);

            node1.LetBool = false;
            link.Push();
            Assert.IsFalse(node2.LetBool);
        }
	}
}
