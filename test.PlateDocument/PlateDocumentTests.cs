using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Nodes;

namespace test.PlateDocument
{
    [TestClass]
    public class PlateDocumentTests
    {
        [TestMethod]
        public void Building_test()
        {
            var blob = new RootBlob();
            var graph = new NodeGraph();
            var parameterSet = new ParameterSet();
            var plateDocument = new PlateDocument();
            var subDocument = new PlateDocument();
            plateDocument.Blob = blob;
            plateDocument.NodeGraph = graph;
            plateDocument.ParameterSet = parameterSet;
            plateDocument.SubDocuments.Add(subDocument);
            blob
            plateDocument.Relationships.Add(new PrototypeRelative())
        }
        NodeGraph GetNodeGraph()
        {
            var graph = new NodeGraph();

            graph.Nodes.Add();
        }
        [TestMethod]
        public void MyTestMethod()
        {
            var graph = new NodeGraph();
            var 
            var textLeafNode = Node.Create()
        }
        
    }
}
