using System;
using System.ComponentModel.Design.Serialization;
using System.Security.Principal;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Nodes;
using psdPH.Nodes.Editor;
using psdPHTest;

namespace test.Nodes.Editor
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var root = new RootBlob();
            var muxNode = new MuxNode();
            root.NodeSet.Nodes.Add(muxNode);
            var w = new Window();
            w.Content =new NodesEditor(root);
            w.ShowDialog();
        }
    }
}
