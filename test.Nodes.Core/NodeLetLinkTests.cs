using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.Nodes;
using psdPH.Reflection;

namespace test.Nodes.Core
{
	[TestClass]
	public class NodeLetLinkTests
	{
		private class Single
		{
			int a;
			public Let Let => new Let(new ReflectionConfig(this,nameof(a)));
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
