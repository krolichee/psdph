using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using psdPH.Nodes.UI;

namespace psdPH.Nodes.CanvasManager
{
    public class LinkPullManager
    {
        public static LinkPullManager Attach(NodeCanvasManager canvasManager)
        {
            return new LinkPullManager(canvasManager);
        }
        NodeSetup pullLink;
        Point dragStartPoint;
        Line PullLinkLine = new Line();
        public NodeSetup PullLink
        {
            get => pullLink;
            set
            {
                Console.WriteLine("SetPullLink");
                if (value != null)
                    dragStartPoint = Mouse.GetPosition(Canvas);
                pullLink = value;
                PullLinkLine.CaptureMouse();
                LinkLine.Paint(PullLinkLine, LinkLineEffect.None);
            }
        }
        public void PreviewLink(NodeSetup nodeSetup)
        {
            LinkLineEffect effect = PullLink?.CanLink(nodeSetup) == true ? LinkLineEffect.None : LinkLineEffect.Bad;
            if (PullLinkLine != null)
                LinkLine.Paint(PullLinkLine, effect);
        }
        public void LinkPulledTo(NodeSetup nodeSetup)
        {
            Console.WriteLine("LinkPulledTo");
            try
            {
                PullLink?.Link(nodeSetup);
            }
            catch (NotCompatibleSetupException)
            {
            }
            PullLink = null;
            Canvas.Children.Remove(PullLinkLine);
        }
        List<NodeUI> NodeUIs = new List<NodeUI>();
        public readonly Canvas Canvas;

        public LinkPullManager(NodeCanvasManager canvasManager)
        {
            Canvas = canvasManager.Canvas;
            canvasManager.NodeUIAdded += CanvasManager_NodeUIAdded;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            Canvas.MouseMove += Canvas_MouseMove;
        }

        private void CanvasManager_NodeUIAdded(NodeUI nodeUI)
        {
            foreach (SetupBar setupBar in nodeUI.SetupBars)
            {
                setupBar.NodeSetupPick += (ns) =>
                PullLink = ns;
                setupBar.NodeSetupHover += PreviewLink;
                setupBar.NodeSetupPut += LinkPulledTo;
            }
            nodeUI.MouseLeave += NodeUI_MouseLeave;
        }
        private void NodeUI_MouseLeave(object sender, MouseEventArgs e)
        {
            LinkLine.Paint(PullLinkLine, LinkLineEffect.None);
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
