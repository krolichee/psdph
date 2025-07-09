using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public partial class NodeCanvasManager
    {
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

        public void PreviewLink(NodeSetup nodeSetup)
        {
            LinkLineEffect effect = PullLink?.CanLink(nodeSetup) == true ? LinkLineEffect.None : LinkLineEffect.Bad;
            if (PullLinkLine != null)
                LinkLine.Paint(PullLinkLine, effect);
        }
        public void LinkPulledTo(NodeSetup nodeSetup)
        {
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
                LinkLine.Paint(PullLinkLine, LinkLineEffect.None);
            }
        }
        private void NodeUI_CanvasDragged(FrameworkElement sender, Vector delta)
        {
            UpdateLines();
        }
        NodeCanvasManager(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            UpdateLines();
        }
        

        

        private void NodeUI_MouseLeave(object sender, MouseEventArgs e)
        {
            LinkLine.Paint(PullLinkLine, LinkLineEffect.None);
        }

        void releasePull()
        {
            PullLink = null;
            Canvas.Children.Remove(PullLinkLine);
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
        private void UpdateLines()
        {
            List<NodeUI> nodeUIs = new List<NodeUI>();
            foreach (var item in Canvas.Children.OfType<UIElement>().ToList())
            {
                if (item is Line line)
                    Canvas.Children.Remove(line);
                if (item is NodeUI nodeUI)
                    nodeUIs.Add(nodeUI);
            }
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
            var nodesSetupBars = nodeUI.SetupBars;
            foreach (SetupBar setupBar in nodesSetupBars)
            {
                setupBar.NodeSetupPick += (ns) => pullLink = ns;
                setupBar.NodeSetupHover += PreviewLink;
                setupBar.NodeSetupPut += LinkPulledTo;
            }
            nodeUI.Node.Links.CollectionChanged += (_, __) => UpdateLines();
            nodeUI.CanvasDragged += NodeUI_CanvasDragged; 
            nodeUI.MouseLeave += NodeUI_MouseLeave;

            if (double.IsNaN(Canvas.GetLeft(nodeUI)))
                Canvas.SetLeft(nodeUI, 0);
            if (double.IsNaN(Canvas.GetTop(nodeUI)))
                Canvas.SetTop(nodeUI, 0);
            Canvas.Children.Add(nodeUI);
            NodeUIAdded?.Invoke(nodeUI);
        }
    }
}
