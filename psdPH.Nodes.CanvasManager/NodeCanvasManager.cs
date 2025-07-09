using psdPH.Nodes.UI;
using psdPH.Nodes.UI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public partial class NodeCanvasManager
    {
        public readonly Canvas Canvas;
        static NodeCanvasManager instance;
        public delegate void NodeUIEvent(NodeUI nodeUI);
        public event NodeUIEvent NodeUIAdded;
        public static NodeCanvasManager Instance()
        {
            if (instance == null)
                throw new Exception();
            return instance;
        }
        public static NodeCanvasManager MakeInstance(Canvas canvas)
        {
            instance = new NodeCanvasManager(canvas);
            return instance;
        }






        NodeCanvasManager(Canvas canvas)
        {
            Canvas = canvas;
            LinkPullManager.Attach(this);
            ChainPullManager.Attach(this);
            SelectionManager.Attach(this);

            UpdateLines();
        }

        class SetupBarsLinks
        {
            public SetupBar From;
            public SetupBar To;

            public SetupBarsLinks(SetupBar from, SetupBar to)
            {
                From = from;
                To = to;
            }
        }
        private void drawSetupLinks(List<NodeUI> nodeUIs)
        {
            List<SetupBar> setupBars = new List<SetupBar>();
            foreach (var item in nodeUIs.Select(nui => nui.SetupBars))
            {
                setupBars.AddRange(item);
            }
            List<SetupBarsLinks> setupBarsLinks = getSetupBarsLinks(setupBars.ToArray());
            foreach (var item in setupBarsLinks)
            {

                var fromBar = item.From;
                var toBar = item.To;

                var line = LinkLine.Create(fromBar, toBar, Canvas);

                Canvas.Children.Add(line);
            }
        }
        private void drawFlowLine(NodeUI source, NodeUI dest)
        {
            var line = LinkLine.Create(source, dest, Canvas);
            Canvas.Children.Add(line);
        }
        private void drawFlowLine(ChainBar chainBar, NodeUI nodeUI)
        {
            var line = LinkLine.Create(chainBar, nodeUI, Canvas);
            Canvas.Children.Add(line);
        }
        private void drawChainLinks(List<NodeUI> nodeUIs)
        {
            foreach (var nodeUI in nodeUIs)
            {
                var node = nodeUI.Node;
                var chain = node.Chain;
                if (chain == null)
                    continue;

                var chainedNode = nodeUIs.First(nui => nui.Node == chain.Node);
                if (chain.Setup.IsNone())
                    drawFlowLine(chainedNode, nodeUI);
                else
                {
                    var chainBar = chainedNode.ChainBars.First(c => c.NodeSetup.Equals(chain));
                    drawFlowLine(chainBar, nodeUI);
                }
            }
        }
        private void UpdateLines()
        {
            List<NodeUI> nodeUIs = new List<NodeUI>();
            foreach (var item in Canvas.Children.OfType<UIElement>().ToList())
            {
                if (item is Line line)
                {
                    Canvas.Children.Remove(line);
                    LinkLine.Clear(line);
                }
                if (item is NodeUI nodeUI)
                    nodeUIs.Add(nodeUI);
            }
            drawSetupLinks(nodeUIs);
            drawChainLinks(nodeUIs);
        }

        private List<SetupBarsLinks> getSetupBarsLinks(SetupBar[] setupBars)
        {
            var result = new List<SetupBarsLinks>();
            for (int i = 0; i < setupBars.Length; i++)
            {
                SetupBar fromBar = setupBars[i];
                var fromSetupOutputLinks = fromBar.NodeSetup.Node.Links.Where(ol => ol.FromNodeSetup.Equals(fromBar.NodeSetup));

                var toNodeSetups = fromSetupOutputLinks.Select(ol => ol.ToNodeSetup).ToArray();

                for (int j = 0; j < toNodeSetups.Length; j++)
                {
                    var toNodeSetup = toNodeSetups[j];
                    SetupBar toBar = setupBars.First(s => s.NodeSetup.Equals(toNodeSetup));
                    result.Add(new SetupBarsLinks(fromBar, toBar));
                }
            }
            return result;
        }

        public void AddNode(Node node)
        {

            var nodeUI = new NodeUI(node);
            NodeUIAdded?.Invoke(nodeUI);


            nodeUI.Node.Links.CollectionChanged += (_, __) => UpdateLines();
            nodeUI.Node.ChainChanged += UpdateLines;
            nodeUI.CanvasDragged += NodeUI_CanvasDragged;


            if (double.IsNaN(Canvas.GetLeft(nodeUI)))
                Canvas.SetLeft(nodeUI, 0);
            if (double.IsNaN(Canvas.GetTop(nodeUI)))
                Canvas.SetTop(nodeUI, 0);
            Canvas.Children.Add(nodeUI);

        }
        private void NodeUI_CanvasDragged(FrameworkElement sender, Vector delta)
        {
            UpdateLines();
        }
    }
}
