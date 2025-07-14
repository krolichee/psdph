using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        RootBlob sampleComposition()
        {
            var result = new RootBlob();
            result.AddChild(new AreaLeaf() { LayerName = "зона1" });
            result.AddChild(new AreaLeaf() { LayerName = "зона2" });
            return result;
        }
        [TestMethod]
        public void testNodeCanvas()
        {
            
            var w = new Window()
            {
                Height = 400,
                Width = 800
            };
            var c = new Canvas()
            {
                Height = 400,
                Width = 800,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Colors.White)
            };
            var scr = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Disabled };
            scr.Content = c;
            var ncm = NodeCanvasManager.MakeInstance(c, scr);
            
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
            
            w.Content = scr;
            w.ShowDialog();
        }
        [TestMethod]
        public void testChainUI()
        {
            var w = new Window()
            {
                Height = 400,
                Width = 800
            };
            var c = new Canvas()
            {
                Height = 2000,
                Width = 2000,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = Brushes.AliceBlue
            };
            var sc = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Hidden ,HorizontalScrollBarVisibility=ScrollBarVisibility.Hidden};
            sc.MaxWidth = 2000;
            sc.MaxHeight = 2000;
            sc.Content = c;
            var ncm = NodeCanvasManager.MakeInstance(c,sc);
            var fpar = new FlagParameter("flag1");
            var fpar_node = new ParameterNode(fpar);
            var forkNode = new ForkNode();
            var ruleNode = new RuleNode(new AlignRule(sampleComposition()));

            ncm.AddNode(fpar_node);
            ncm.AddNode(ruleNode);
            ncm.AddNode(forkNode);
            
            w.Content = sc;
            w.ShowDialog();
        }
    }
}
