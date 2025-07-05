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
using psdPH.Nodes.Nodes;
using psdPH.Nodes.UI;
using psdPHTest;
using System.Windows.Interactivity;

namespace test.Nodes.UI
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class NodeUITest
    {
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
                Background = new SolidColorBrush(Colors.White)
            };
            var n = new MuxNode();
            var n2 = new MuxNode();
            var nui = new NodeUI(n)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            var nui2 = new NodeUI(n2)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            var on = new ParameterNode(new StringParameter("Хавальник"));
            var composition = new RootBlob();
            composition.AddChild(new AreaLeaf() { LayerName = "зона1"});
            composition.AddChild(new AreaLeaf() { LayerName = "зона2"});
            var ll = new LayerLeaf() { LayerName = "слой" };
            composition.AddChild(ll);
            var rule = new AlignRule(composition);
            var rn = new RuleNode(rule);
            var llon = new ObjectNode(ll);
            var nui3 = new NodeUI(on);
            var nui4 = new NodeUI(rn);
            var nui5 = new NodeUI(llon);

            var textCondition = new EmptynessCondition();
            var empNode = new ConditionNode(textCondition);
            var nui6 = new NodeUI(empNode);

            c.Children.Add(nui);
            c.Children.Add(nui2);
            c.Children.Add(nui3);
            c.Children.Add(nui4);
            c.Children.Add(nui5);
            c.Children.Add(nui6);
            NodeConvasManager.MakeInstance(c);
            w.Content = c;
            w.ShowDialog();
        }
    }
}
