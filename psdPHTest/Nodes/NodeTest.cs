using System;
using System.Linq;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Navigation;
using psdPH.Views.WeekView.Logic;
using psdPH;
using psdPH.Nodes;
using System.Security.AccessControl;
using System.Collections.ObjectModel;
using psdPH.Utils;
using psdPH.Logic;
using psdPH.Parameters;
using psdPH.Nodes.Core;

namespace psdPHTest.Nodes
{
    [TestCategory(TestCategories.Automatic)]
    [TestClass]
    public class NodeTest
    {
        
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

            fparNode.Link(fparNode.Inputs[0], muxNode, muxNode.ToggleSetup);
            muxNode.Link(muxNode.Outputs[0], sparNode, spar_setup);
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
            inSParNode.Link(inSParNode.Inputs[0],splitNode, dryTextSetup);
            splitNode.Ratio = 1.0 / 1;
            var outSPar = new StringParameter() {Text = "1"} ;
            var outSParNode = new ParameterNode(outSPar);
            splitNode.Link(splitNode.Outputs[0],outSParNode, outSParNode.Inputs[0]);
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
                fpar1Node.Link(fpar1Node.Inputs[0], fpar2Node, fpar2Node.Inputs[0]);
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
            fpar1Node.Link(fpar1Node.Inputs[0], fpar2Node, fpar2Node.Inputs[0]);
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
