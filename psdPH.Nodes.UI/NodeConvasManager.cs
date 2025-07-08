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
    public class NodeConvasManager
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
            LinkLineEffect effect = DraggedLink?.CanLink(nodeSetup) == true ? LinkLine.LinkLineEffect.None : LinkLine.LinkLineEffect.Bad;
            if (Line!=null)
                LinkLine.Paint(Line, effect);
        }
        public void LinkDraggedTo(NodeSetup nodeSetup)
        {
            try
            {
                DraggedLink?.Link(nodeSetup);
            }
            catch (NotCompatibleSetupException) {
            }
            DraggedLink = null;
            Canvas.Children.Remove(Line);
        }
        List<NodeUI> NodeUIs = new List<NodeUI>();

        Canvas Canvas;
        NodeSetup draggedLink;
        Point startPoint;
        Line Line;
        public NodeSetup DraggedLink
        {
            get => draggedLink;
            set
            {
                if (value!=null)
                    startPoint = Mouse.GetPosition(Canvas);
                draggedLink = value;
                Line.CaptureMouse();
                LinkLine.Paint(Line, LinkLine.LinkLineEffect.None);
            }
        }
        NodeConvasManager(Canvas canvas)
        {
            Line = new Line();
            Canvas = canvas;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            foreach (FrameworkElement item in Canvas.Children)
            {
                if (item is NodeUI nodeUI)
                { 
                    nodeUI.Node.Links.CollectionChanged += (_, __) => UpdateLines();
                    nodeUI.CanvasDragged+= UpdateLines;

                    nodeUI.MouseLeave += NodeUI_MouseLeave;
                }
                if (double.IsNaN(Canvas.GetLeft(item)))
                    Canvas.SetLeft(item, 0);
                if (double.IsNaN(Canvas.GetTop(item)))
                    Canvas.SetTop(item, 0);
                


            }
            UpdateLines();
            //canvas.MouseUp += Canvas_MouseLeftButtonUp;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NodeUI_MouseLeave(object sender, MouseEventArgs e)
        {
            LinkLine.Paint(Line, LinkLineEffect.None);
        }

        void releaseDrag()
        {
            DraggedLink = null;
            Canvas.Children.Remove(Line);
        }
        private void Canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            releaseDrag();
        }
        bool selection;
        Rectangle selectionRectangle;
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            if (DraggedLink != null)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    releaseDrag();
                    return;
                }
                Canvas.Children.Remove(Line);
                var endPoint = e.GetPosition(Canvas);
                var shift = startPoint.X > endPoint.X ? 1 : -1;
                Line.X1 = startPoint.X;
                Line.Y1 = startPoint.Y;
                Line.X2 = endPoint.X + shift;
                Line.Y2 = endPoint.Y;
                Canvas.Children.Add(Line);
            }
            else if (selection)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    releaseSelect();
                    return;
                }
                
            }
            
        }

        private void releaseSelect()
        {
            throw new NotImplementedException();
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

                var line = LinkLine.Create(fromBar,toBar,Canvas);

                Canvas.Children.Add(line);
            }
        }

        private List<SetupBarsLinks> getSetupBarsLinks(SetupBar[] setupBars)
        {
            var result = new List<SetupBarsLinks>();
            for (int i = 0; i < setupBars.Length; i++)
            {
                SetupBar fromBar = setupBars[i];
                var fromSetupOutputLinks = fromBar.NodeSetup.Node.Links.Where(ol=>ol.FromNodeSetup.Equals(fromBar.NodeSetup));

                var toNodeSetups = fromSetupOutputLinks.Select(ol => ol.ToNodeSetup).ToArray();

                for (int j = 0; j < toNodeSetups.Length; j++)
                {
                    var toNodeSetup = toNodeSetups[j];
                    SetupBar toBar = setupBars.First(s=>s.NodeSetup.Equals(toNodeSetup));
                    result.Add(new SetupBarsLinks(fromBar,toBar));
                }
            }
            return result;
        }
    }
}
