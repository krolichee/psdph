using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Nodes;
using psdPH.Utils;
using psdPH.Parameters;
using psdPH.Nodes.Core;
using psdPH.Nodes.Nodes;

namespace psdPHTest.Nodes
{
    [TestCategory(TestCategories.Automatic)]
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void ChainTest()
        {
            object obj = 1;
            var obj_node = new ObjectNode(obj);
            var spar1 = new StringParameter();
            var spar1_node = new ParameterNode(spar1);
            var spar2 = new StringParameter();
            var spar2_node = new ParameterNode(spar2);
            spar1.Text = "1";
            spar1_node.ChainIn(obj_node);
            spar1_node.LinkOut(spar1_node.IOSetups[0], spar2_node, spar2_node.IOSetups[0]);
            obj_node.Apply();
            Assert.IsTrue(spar1.Text == spar2.Text);
        }
        [TestMethod]
        public void ForkNodeTest()
        {
            Node forkNode = new ForkNode();
            var fpar = new FlagParameter();
            
            var fpar_node = new ParameterNode(fpar);
            var result1Fpar = new FlagParameter() { Name = "result1"};
            var result1Fpar_node = new ParameterNode(result1Fpar);
            var result2Fpar = new FlagParameter() { Name = "result2" };
            var result2Fpar_node = new ParameterNode(result2Fpar);

            var aux1fpar_node = new ParameterNode( new FlagParameter() { Value = true ,Name = "aux1"});
            var aux2fpar_node = new ParameterNode( new FlagParameter() { Value = true, Name = "aux2" });

            fpar_node.LinkOut(fpar_node.Inputs[0],forkNode, forkNode.Inputs[0]);
            aux1fpar_node.ChainIn(new NodeSetup( forkNode, forkNode.Chains[0]));
            aux2fpar_node.ChainIn(new NodeSetup(forkNode, forkNode.Chains[1]));

            aux1fpar_node.LinkOut(aux1fpar_node.IOSetups[0], result1Fpar_node, result1Fpar_node.IOSetups[0]);
            aux2fpar_node.LinkOut(aux2fpar_node.IOSetups[0], result2Fpar_node, result2Fpar_node.IOSetups[0]);

            fpar.Toggle = true;
            fpar_node.Apply();

            Assert.IsTrue(result1Fpar.Toggle == true, "Первый чейн не выполнен");
            Assert.IsTrue(result2Fpar.Toggle != true,"Второй был выполнен вместе с первым");
            fpar.Toggle = false;
            fpar_node.Apply();
            Assert.IsTrue(result2Fpar.Toggle, "Второй чейн не выполнен");

        }
        
        [TestMethod]
        public void ParameterNode()
        {
            var fpar = new FlagParameter();
            var parNode = new ParameterNode(fpar);
            
        }
        [TestMethod]
        public void NodeCouple()
        {
            var fpar = new FlagParameter();
            var fparNode = new ParameterNode(fpar);
            var spar = new StringParameter();
            var sparNode = new ParameterNode(spar);
            var muxNode = new MuxNode();

            //var fpar_setup = fpar.Setups.First(s => s.Config.FieldName == nameof(Parameter.Value));
            var spar_setup = sparNode.Inputs[0];// spar.Setups.First(s => s.Config.FieldName == nameof(Parameter.Value));

            fparNode.LinkOut(fparNode.Inputs[0], muxNode, muxNode.ToggleSetup);
            muxNode.LinkOut(muxNode.Outputs[0], sparNode, spar_setup);
            muxNode.OnObj="on";
            muxNode.OffObj="off";

            fpar.Toggle = true;
            fparNode.Apply();
            Assert.IsTrue(spar.Text == "on");

            fpar.Toggle = false;
            fparNode.Apply();
            Assert.IsTrue(spar.Text == "off");
        }
        
        [TestMethod]
        public void SplitNode()
        {
            var text = "лучшие книги 2024 года";
            var inSPar = new StringParameter() { Text = text };
            var inSParNode = new ParameterNode(inSPar);
            var splitNode = new SplitForRatioNode();
            var dryTextSetup = splitNode.Inputs[0];
            inSParNode.LinkOut(inSParNode.Inputs[0],splitNode, dryTextSetup);
            splitNode.Ratio = 1.0 / 1;
            var outSPar = new StringParameter() {Text = "1"} ;
            var outSParNode = new ParameterNode(outSPar);
            splitNode.LinkOut(splitNode.Outputs[0],outSParNode, outSParNode.Inputs[0]);
            inSParNode.Apply();
            Assert.IsTrue(outSPar.Text != "1");
            Assert.IsTrue(outSPar.Text != text);
        }
        
        [TestMethod]
        public void Serialization()
        {

            string xml;
            {
                ParameterSet parameterSet = new ParameterSet();
                var fpar1 = new FlagParameter();
                var fpar1Node = new ParameterNode(fpar1);
                var fpar2 = new FlagParameter();
                var fpar2Node = new ParameterNode(fpar2);
                fpar1Node.LinkOut(fpar1Node.Inputs[0], fpar2Node, fpar2Node.Inputs[0]);
                parameterSet.Add(fpar1);
                parameterSet.Add(fpar2);
                var nodeSet = new NodeSet();
                nodeSet.Nodes.Add(fpar1Node);
                nodeSet.Nodes.Add(fpar2Node);
                var root = new RootBlob();
                root.NodeSet = nodeSet;
                root.ParameterSet = parameterSet;
                xml = CloneConverter.GetXml(root);
            }
            {
                var root = CloneConverter.GetObj<RootBlob>(xml);
                var fpar1 = root.ParameterSet[0];
                var fpar2 = root.ParameterSet[1];
                var fpar1Node = root.NodeSet[0];
                var fpar2Node = root.NodeSet[1];
                testFlagQualityAfterApplyNode(fpar1Node as ParameterNode, fpar2Node as ParameterNode);
            }


        }
        [TestMethod]
        public void Throught()
        {
            var fpar1 = new FlagParameter();
            var fpar1Node = new ParameterNode(fpar1);
            var fpar2 = new FlagParameter();
            var fpar2Node = new ParameterNode(fpar2);
            fpar1Node.LinkOut(fpar1Node.Inputs[0], fpar2Node, fpar2Node.Inputs[0]);
            testFlagQualityAfterApplyNode(fpar1Node, fpar2Node);
        }

        private static void testFlagQualityAfterApplyNode(ParameterNode fpar1Node, ParameterNode fpar2Node)
        {
            var fpar1 = fpar1Node.Parameter as FlagParameter;
            var fpar2 = fpar2Node.Parameter as FlagParameter;
            fpar1.Toggle = true;
            fpar1Node.Apply();
            Assert.AreEqual(fpar1.Value, fpar2.Value);
            fpar1.Toggle = false;
            fpar1Node.Apply();
            Assert.AreEqual(fpar1.Value, fpar2.Value);
        }

        [TestMethod]
        public void SetupHash()
        {
            var spar = new StringParameter();
            var s1 = spar.Setups[0];
            Assert.IsTrue(spar.Setups.Contains(s1));
        }
    }
}
