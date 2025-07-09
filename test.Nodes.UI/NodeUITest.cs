using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Compositions;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Nodes;
using psdPH.Nodes.CanvasManager;
using psdPH.Nodes.Nodes;
using psdPH.Nodes.UI;
using psdPHTest;

namespace test.Nodes.UI
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class NodeUITest
    {
        [TestMethod]
        public void testNodeCanvas()
        {
            RootBlob sampleComposition()
            {
                var result = new RootBlob();
                result.AddChild(new AreaLeaf() { LayerName = "зона1" });
                result.AddChild(new AreaLeaf() { LayerName = "зона2" });
                return result;
            }
            var w = new Window()
            {
                Height = 400,
                Width = 800
            };

            

            var c = new Canvas()
            {
                Background = new SolidColorBrush(Colors.White)
            };
            var ncm = NodeCanvasManager.MakeInstance(c);
            
            var parameterNode = new ParameterNode(new StringParameter("Хавальник"));

            var composition = sampleComposition();

            var layerLeaf = new LayerLeaf() { LayerName = "слой" };
            composition.AddChild(layerLeaf);

            var alignRule = new AlignRule(composition);
            var textCondition = new EmptynessCondition();

            var muxNode1 = new MuxNode();
            var muxNode2 = new MuxNode();
            var alignRuleNode = new RuleNode(alignRule);
            var layerLeafNode = new ObjectNode(layerLeaf);
            
            var empNode = new ConditionNode(textCondition);

            ncm.AddNode(muxNode1);
            ncm.AddNode(muxNode2);
            ncm.AddNode(alignRuleNode);
            ncm.AddNode(layerLeafNode);
            ncm.AddNode(empNode);
            var scr = new ScrollViewer();
            scr.Content = c;
            w.Content = scr;
            w.ShowDialog();
        }
    }
}
