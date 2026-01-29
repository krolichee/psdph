using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace psdPH.Nodes.CanvasManager
{
    public class PanManager
    {
        Canvas canvas;
        ScrollViewer scrollViewer;

        public static PanManager Attach(Canvas canvas, ScrollViewer scrollViewer)
        {
           return new PanManager(canvas, scrollViewer);
        }
        PanManager() { }
        PanManager(Canvas canvas, ScrollViewer scrollViewer)
        {
            this.canvas = canvas;
            this.scrollViewer = scrollViewer;
            canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            canvas.MouseRightButtonUp += Canvas_MouseRightButtonUp;
            canvas.MouseMove += Canvas_MouseMove;
        }

        private Point _lastMousePosition;
        private bool _isDragging = false;

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(scrollViewer);
                canvas.CaptureMouse();
                e.Handled = true;
            }
        }

        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                canvas.ReleaseMouseCapture();
                e.Handled = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.RightButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(scrollViewer);
                Vector offset = _lastMousePosition - currentPosition;
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + offset.X);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset.Y);

                _lastMousePosition = currentPosition;
                e.Handled = true;
            }
        }
    }
}
