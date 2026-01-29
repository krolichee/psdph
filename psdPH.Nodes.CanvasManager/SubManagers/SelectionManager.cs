using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public class SelectionManager
    {

        public static SelectionManager Attach(NodeCanvasManager canvasManager)
        {
            return new SelectionManager(canvasManager);
        }
        NodeCanvasManager CanvasManager;
        Canvas Canvas=> CanvasManager.Canvas;
        Rect getSelectionRect()
        {
            return selectionBorder.GetCanvasRect();
        }
        bool selection;
        Border selectionBorder = new Border() { Background = Brushes.Blue, Opacity = 0.4 };
        Point selectionStartPoint;

        List<Border> elementSelectionBorders = new List<Border>();
        void drawSelection(FrameworkElement e)
        {
            if (e is NodeUI nodeUI)
            {
                nodeUI.Selected = true;
                return;
                var esBorder = new Border() { Background = Brushes.Blue, Opacity = 0.4 };
                esBorder.SetCanvasRect(nodeUI.GetCanvasRect());
                elementSelectionBorders.Add(esBorder);
                Canvas.Children.Add(esBorder);
            }
            else if (e is Line line)
                ConnectionLineDrawer.Paint(line, LinkLineEffect.Selected);
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
            selectionBorder.SetCanvasRect(new Rect(topLeft, bottomRight));
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
                
                if (element is Line line)
                    return GeometryHelper.LineIntersectsRect(new Point(line.X1, line.Y1), new Point(line.X2, line.Y2), selectionRect);
                var elementRect = element.GetCanvasRect();
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
                    ConnectionLinkLineModel.Get(line).Selected = false;
                else if (item is NodeUI nodeUI)
                    nodeUI.Selected = false;
            }
            elementSelectionBorders.Clear();
        }
        List<FrameworkElement> selectedElements = new List<FrameworkElement>();

        public SelectionManager(NodeCanvasManager canvasManager)
        {
            CanvasManager = canvasManager;
            Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            Canvas.MouseMove += Canvas_MouseMove;
            CanvasManager.NodeUIAdded += CanvasManager_NodeUIAdded;
            Canvas.PreviewKeyDown += Canvas_KeyDown;
            Canvas.KeyDown += Canvas_KeyDown;
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && selectedElements.Count!=0)
            {
                Delete();
                e.Handled = true;
            }
        }

        private void CanvasManager_NodeUIAdded(NodeUI nodeUI)
        {
            nodeUI.CanvasDragged += NodeUI_CanvasDragged;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
             if (selection)
                if (e.LeftButton == MouseButtonState.Released)
                    releaseSelection();
                else
                {
                    previewSelectionBorder(e.GetPosition(Canvas));
                    previewSelection();
                }


        }
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
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            releaseSelection();
            Canvas.Focus();
        }
        void canvasDrag(FrameworkElement element, Vector delta)
        {
            element.SetCanvasPoint(element.GetCanvasPoint() + delta);
        }
        private void NodeUI_CanvasDragged(FrameworkElement sender, Vector delta)
        {
            if (!selectedElements.Contains(sender))
                clearSelection();
            else
                foreach (var item in selectedElements.Where(e => e != sender))
                    canvasDrag(item, delta);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearSelection();
            clearSelectionPreview();
            selectionStartPoint = e.GetPosition(Canvas);
            selection = true;
            Canvas.Children.Add(selectionBorder);
            previewSelectionBorder(e.GetPosition(Canvas));
        }
        public void Delete()
        {
                CanvasManager.DeleteElementFromModel(selectedElements.ToArray());
        }
    }
}
