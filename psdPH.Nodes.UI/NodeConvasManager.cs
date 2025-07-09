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
using static psdPH.Nodes.UI.LinkLine;

namespace psdPH.Nodes.UI
{
    public partial class NodeConvasManager
    {
        static NodeConvasManager instance;
        public static NodeConvasManager Instance()
        {
            if (instance == null)
                throw new Exception();
            return instance;
        }
        public static NodeConvasManager MakeInstance(Canvas canvas)
        {
            instance = new NodeConvasManager(canvas);
            return instance;
        }

        public void PreviewLink(NodeSetup nodeSetup)
        {
            LinkLineEffect effect = PullLink?.CanLink(nodeSetup) == true ? LinkLine.LinkLineEffect.None : LinkLine.LinkLineEffect.Bad;
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

        Canvas Canvas;
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
                LinkLine.Paint(PullLinkLine, LinkLine.LinkLineEffect.None);
            }
        }
        void canvasDrag(FrameworkElement element, Vector delta)
        {
            setCanvasPoint(element, getCanvasPoint(element) + delta);
        }
        private void NodeUI_CanvasDragged(FrameworkElement sender, Vector delta)
        {
            if (!selectedElements.Contains(sender))
                clearSelection();
            else
                foreach (var item in selectedElements.Where(e => e != sender))
                    canvasDrag(item, delta);
            UpdateLines();
        }
        NodeConvasManager(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            UpdateLines();
        }
        

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearSelection();
            clearSelectionPreview();
            selectionStartPoint = e.GetPosition(null);
            selection = true;
            Canvas.Children.Add(selectionBorder);
            previewSelectionBorder(e.GetPosition(null));
        }

        private void NodeUI_MouseLeave(object sender, MouseEventArgs e)
        {
            LinkLine.Paint(PullLinkLine, LinkLineEffect.None);
        }

        void releaseDrag()
        {
            PullLink = null;
            Canvas.Children.Remove(PullLinkLine);
        }
        private void Canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            releaseDrag();
            releaseSelection();
        }
        bool selection;
        Border selectionBorder = new Border() { Background = Brushes.Blue, Opacity = 0.4 };
        void previewDrag(MouseEventArgs e)
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
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (PullLink != null)
                if (e.LeftButton == MouseButtonState.Released)
                    releaseDrag();
                else
                    previewDrag(e);
            else if (selection)
                if (e.LeftButton == MouseButtonState.Released)
                    releaseSelection();
                else
                {
                    previewSelectionBorder(e.GetPosition(null));
                    previewSelection();
                }


        }
        Point getCanvasPoint(FrameworkElement e)
        {
            return new Point(Canvas.GetLeft(e), Canvas.GetTop(e));
        }
        void setCanvasPoint(FrameworkElement e, Point point)
        {
            Canvas.SetLeft(e, point.X);
            Canvas.SetTop(e, point.Y);
        }
        Rect getSelectionRect()
        {
            return getCanvasRect(selectionBorder);
        }
        Rect getCanvasRect(FrameworkElement e)
        {
            return new Rect(getCanvasPoint(e), e.RenderSize);
        }
        void setCanvasRect(FrameworkElement e, Rect rect)
        {
            setCanvasPoint(e, rect.TopLeft);
            e.Width = rect.Width;
            e.Height = rect.Height;
        }
        Point selectionStartPoint;

        List<Border> elementSelectionBorders = new List<Border>();
        void drawSelection(FrameworkElement e)
        {
            if (e is NodeUI nodeUI)
            {
                nodeUI.Selected = true;
                return;
                var esBorder = new Border() { Background = Brushes.Blue, Opacity = 0.4 };
                setCanvasRect(esBorder, getCanvasRect(nodeUI));
                elementSelectionBorders.Add(esBorder);
                Canvas.Children.Add(esBorder);
            }
            else if (e is Line line)
                LinkLine.Paint(line, LinkLineEffect.Selected);
        }

        void setupSelectionBorder(Point endPoint)
        {
            var topLeft = new Point();
            var bottomRight = new Point();
            double[] xs = new[] { selectionStartPoint.X, endPoint.X };
            double[] ys = new[] { selectionStartPoint.Y, endPoint.Y };
            Array.Sort(xs);
            Array.Sort(ys);
            topLeft.X = xs.First();
            topLeft.Y = ys.First();
            bottomRight.X = xs.Last();
            bottomRight.Y = ys.Last();
            setCanvasRect(selectionBorder, new Rect(topLeft, bottomRight));
        }
        void previewSelectionBorder(Point endPoint)
        {
            setupSelectionBorder(endPoint);
        }
        void drawSelectionPreview()
        {
            foreach (var item in selectedElements)
                drawSelection(item);
        }
        void previewSelection()
        {
            clearSelectionPreview();
            if (selection)
            {
                addToSelection();
                drawSelectionPreview();
            }
        }

        private void addToSelection()
        {
            bool inSelectionBorder(FrameworkElement element)
            {
                var selectionRect = getSelectionRect();
                var elementRect = getCanvasRect(element);
                if (element is Line line)
                    elementRect = new Rect(new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
                return selectionRect.IntersectsWith(elementRect);
            }
            selectedElements.Clear();
            foreach (FrameworkElement item in Canvas.Children)
                if (inSelectionBorder(item))
                    selectedElements.Add(item);
        }

        private void clearSelectionPreview()
        {

            //foreach (var item in elementSelectionBorders)
            //    Canvas.Children.Remove(item);


            foreach (var item in Canvas.Children)
            {
                if (item is Line line)
                    LinkLine.Paint(line, LinkLineEffect.None);
                else if (item is NodeUI nodeUI)
                    nodeUI.Selected = false;
            }
            elementSelectionBorders.Clear();
        }
        List<FrameworkElement> selectedElements = new List<FrameworkElement>();
        private void clearSelection()
        {
            selectedElements.Clear();
            previewSelection();
        }
        private void releaseSelection()
        {
            selection = false;
            Canvas.Children.Remove(selectionBorder);
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
            nodeUI.Node.Links.CollectionChanged += (_, __) => UpdateLines();
            nodeUI.CanvasDragged += NodeUI_CanvasDragged; 
            nodeUI.MouseLeave += NodeUI_MouseLeave;

            if (double.IsNaN(Canvas.GetLeft(nodeUI)))
                Canvas.SetLeft(nodeUI, 0);
            if (double.IsNaN(Canvas.GetTop(nodeUI)))
                Canvas.SetTop(nodeUI, 0);
            Canvas.Children.Add(nodeUI);
        }
    }
}
