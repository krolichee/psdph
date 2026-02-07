using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;
using psdPH.Nodes.Basic;
using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Nodes.UI
{
    [TestClass]
    public class NodeControlViewModelTests
    {
        [TestMethod]
        public void DeleteTest()
        {
            
            var node = new TernarNode();
            var model = new NodeControlViewModel(node);
            var graph = new NodeGraph();
            NodeCanvasDispatcherGlobal.Instance = new NodeCanvasDispatcher(graph);
            graph.Nodes.Add(node);
            model.DeleteNode();
            Assert.IsFalse(graph.Nodes.Contains(node));
        }
    }
}
