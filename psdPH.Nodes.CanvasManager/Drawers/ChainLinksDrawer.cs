using psdPH.Nodes.UI.UI;
using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Nodes.CanvasManager
{
    class ChainLinksDrawer
    {
        Canvas Canvas;

        public ChainLinksDrawer(Canvas canvas)
        {
            Canvas = canvas;
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
        public void DrawChainLinks(List<NodeUI> nodeUIs)
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
    }
}
