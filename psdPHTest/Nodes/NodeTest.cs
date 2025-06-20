using System;
using System.Linq;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Navigation;
using psdPH.Views.WeekView.Logic;
using psdPH;
using psdPH.Nodes;
using psdPH.Utils;
using System.Security.AccessControl;

namespace psdPHTest.Nodes
{
    [TestCategory(TestCatagories.Automatic)]
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        private void MuxNodeAppliable()
        {
            var muxNode = new MuxNode();
            var fpar = new FlagParameter();
            var sparNode = new ParameterNode( new StringParameter());
            
            var fparNode = new ParameterNode(fpar);
            var fpar_setup = fparNode.Inputs[0];           
            Assert.ThrowsException<Exception>(()=>
                muxNode.Link(muxNode.Outputs[0], fparNode, sparNode.Inputs[0])
            );
            muxNode.Link(muxNode.Outputs[0], fparNode, fparNode.Inputs[0]);
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
        public void MuxNodeApply()
        {
            var muxNode = new MuxNode();
            var spar = new StringParameter();
            var node = new ParameterNode(spar);
            muxNode.OnObj = "on";
            muxNode.OffObj = "off";
            var spar_setup = spar.Setups.First(s => s.Config.FieldName == nameof(Parameter.Value));
            muxNode.Link(muxNode.Outputs[0], node, spar_setup);
            muxNode.Toggle = true;
            muxNode.Apply();
            Assert.IsTrue(spar.Text == "on");
            muxNode.Toggle = false;
            muxNode.Apply();
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
                var fpar = new FlagParameter();
                var fpar1Node = new ParameterNode(fpar);
                var fpar1 = new FlagParameter();
                var fpar2Node = new ParameterNode(fpar1);
                fpar1Node.Link(fpar1Node.Inputs[0], fpar2Node, fpar2Node.Inputs[0]);
                parameterSet.Add(fpar);
                parameterSet.Add(fpar1);
                List<Node> nodes = new List<Node> {  fpar1Node, fpar2Node };
                
            }

        }
        [TestMethod]
        public void Throught()
        {
            var fpar1 = new FlagParameter();
            var fpar1Node = new ParameterNode(fpar1);
            var fpar = new FlagParameter();
            var fparNode = new ParameterNode(fpar);
            fparNode.Link(fparNode.Inputs[0], fpar1Node, fpar1Node.Inputs[0]);
            fpar.Toggle = true;
            fparNode.Apply();
            Assert.AreEqual(fpar1.Value,fpar.Value);
            fpar.Toggle = false;
            fparNode.Apply();
            Assert.AreEqual(fpar1.Value, fpar.Value);
        }
        [TestMethod]
        public void SetupHash()
        {
            var spar = new StringParameter();
            var s1 = spar.Setups[0];
            Assert.IsTrue(spar.Setups.Contains(s1));
        }
    }
    public class SplitForRatioNode:Node
    {
        public string DryText;
        public double Ratio;
        public string WetText;
        public override List<Setup> Inputs => new List<Setup>() { 
            new StringInputSetup(new SetupConfig(this,nameof(DryText))),
            ///TODO RatioSetup
            Setup.TypeConstrained<double>(new SetupConfig(this,nameof(Ratio)))
        };

        public override List<Setup> Outputs => new List<Setup>() { 
            Setup.Sealed(new SetupConfig(this, nameof(WetText))) 
        };

        public SplitForRatioNode()
        {
        }

        protected override void _apply()
        {
            WetText = SplitTextToRatio.Splitter.Split(DryText, Ratio);
        }
    }
    
    public class NodeLink
    {
        Guid FromGuid;
        int FromSetupConfigHash;
        Guid ToGuid;
        int ToSetupConfigHash;
    }
}
