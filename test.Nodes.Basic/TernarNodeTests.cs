using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes.Basic;

namespace test.Nodes.Basic
{
    [TestClass]
    public class TernarNodeTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var node = new TernarNode();
            node.TrueLet.Value = 3;
            node.FalseLet.Value = 5;

            node.FactorLet.Value = true;
            node.Execute();
            Assert.IsTrue(node.OutputLet.Value.Equals(3));
            node.FactorLet.Value = false;
            node.Execute();
            Assert.IsTrue(node.OutputLet.Value.Equals(5));
        }
    }
}
