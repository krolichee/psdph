using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Parameters;
using psdPH.Nodes;
using psdPH.Parameters;

namespace psdPHTest.Nodes
{
    [TestCategory(TestCategories.Automatic)]
	[TestClass]
	public class MuxNodeTest
	{
        [TestMethod]
        public void typeDescriptorTest()
        {
            Assert.IsTrue(typeof(Parameter).IsAssignableFrom(typeof(Parameter)));
            Assert.IsFalse(typeof(StringParameter).IsAssignableFrom(typeof(Parameter)));
            Assert.IsTrue(typeof(Parameter).IsAssignableFrom(typeof(StringParameter)));
        }
        //[TestMethod]
        public void MuxNodeAppliable()
        {
            var muxNode = new MuxNode();
            var fpar = new FlagParameter();
            var sparNode = new ParameterNode(new StringParameter());

            var fparNode = new ParameterNode(fpar);
            var fpar_setup = fparNode.Inputs[0];
            Assert.ThrowsException<NotCompatibleSetupException>(() =>
                muxNode.Link(muxNode.Outputs[0], sparNode, sparNode.Inputs[0])
            );
            muxNode.Link(muxNode.Outputs[0], fparNode, fparNode.Inputs[0]);
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
    }
}
