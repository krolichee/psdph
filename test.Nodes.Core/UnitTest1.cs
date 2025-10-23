using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;
using psdPH.Nodes.Core;
using psdPH.Utils;

namespace test.Nodes.Core
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NodeSetSerialization()
        {
            NodeSet ns = new NodeSet();
            ns.Nodes.Add(new MuxNode());
            CloneConverter.GetXml(ns);
        }
    }
}
