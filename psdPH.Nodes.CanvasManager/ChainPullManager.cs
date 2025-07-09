using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using psdPH.Setups;
using psdPH.Nodes.UI.UI;

namespace psdPH.Nodes.CanvasManager
{
    public class ChainPullManager
    {
        public static ChainPullManager Attach(NodeCanvasManager canvasManager)
        {
            return new ChainPullManager(canvasManager);
        }
        NodeSetup pullLink;
        Point dragStartPoint;
        Line PullLinkLine = new Line();
        public NodeSetup PullLink
        {
            get => pullLink;
            set
            {
                if (value != null)
                    dragStartPoint = Mouse.GetPosition(Canvas);
                pullLink = value;
                PullLinkLine.CaptureMouse();
                LinkLine.Paint(PullLinkLine, LinkLineEffect.Flow);
            }
        }
        public void LinkPulledTo(Node node)
        {
            try
            {
                if (pullLink!=null)
                    node.ChainIn(pullLink);
            }
            catch (NotCompatibleSetupException)
            {
            }
            PullLink = null;
            Canvas.Children.Remove(PullLinkLine);
        }
        List<NodeUI> NodeUIs = new List<NodeUI>();
        public readonly Canvas Canvas;

        public ChainPullManager(NodeCanvasManager canvasManager)
        {
            Canvas = canvasManager.Canvas;
            canvasManager.NodeUIAdded += CanvasManager_NodeUIAdded;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            Canvas.MouseMove += Canvas_MouseMove;
        }

        private void CanvasManager_NodeUIAdded(NodeUI nodeUI)
        {
            nodeUI.ChainPick += NodeUI_ChainPick;
            nodeUI.ChainPut += LinkPulledTo;
            foreach (ChainBar setupBar in nodeUI.ChainBars)
            {
                setupBar.ChainPick += (ns) =>
                PullLink = ns;
            }
        }

        private void NodeUI_ChainPick(Node node)
        {
            PullLink = new NodeSetup(node,Setup.None);
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            releasePull();
        }

        void previewPull(MouseEventArgs e)
        {
            Canvas.Children.Remove(PullLinkLine);
            var endPoint = e.GetPosition(Canvas);
            var shift = dragStartPoint.X > endPoint.X ? 1 : -1;
            PullLinkLine.X1 = dragStartPoint.X;
            PullLinkLine.Y1 = dragStartPoint.Y;
            PullLinkLine.X2 = endPoint.X + shift;
            PullLinkLine.Y2 = endPoint.Y;
            Canvas.Children.Add(PullLinkLine);
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (PullLink != null)
                if (e.LeftButton == MouseButtonState.Released)
                    releasePull();
                else
                    previewPull(e);

        }
        void releasePull()
        {
            PullLink = null;
            Canvas.Children.Remove(PullLinkLine);
        }
    }
}
