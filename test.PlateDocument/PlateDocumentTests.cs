using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Nodes;
using psdPH.Parameters;

namespace test.PlateDocument
{
    [TestClass]
    public class PlateDocumentTests
    {
        [TestMethod]
        public void Building_test()
        {
            var blob = new RootBlob();
            blob.Hierarchy.AddChild(new LayerBlob());
            blob.Hierarchy.AddChild(new LayerLeaf());
            blob.Hierarchy.AddChild(new LayerLeaf());
            var graph = new NodeGraph();
            var parameterSet = new ParameterSet();
            var plateDocument = new PlateDocument();
            var subDocument = new PlateDocument();
            plateDocument.Blob = blob;
            plateDocument.NodeGraph = graph;
            plateDocument.ParameterSet = parameterSet;
            plateDocument.SubDocuments.Add(subDocument);

            var subBlob = blob.Hierarchy.GetChildren<LayerBlob>().First();
            var subGraph = new NodeGraph();
            var subParameterSet = new ParameterSet();
            subDocument.Blob = subBlob;
            subDocument.NodeGraph = subGraph;
            subDocument.ParameterSet = subParameterSet;

            var layerLeaf1 = blob.Hierarchy.GetChildren<LayerLeaf>().First();
            var layerLeaf2 = blob.Hierarchy.GetChildren<LayerLeaf>()[1];
            plateDocument.Relationships.Add(new PrototypeRelative(subBlob, layerLeaf1));
            plateDocument.Relationships.Add(new PlaceholderPrototype(layerLeaf2,subBlob));

            
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
