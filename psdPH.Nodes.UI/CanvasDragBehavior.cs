using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.Nodes.UI
{
    public class CanvasDragBehavior
    {
        public Canvas Canvas;
        public delegate void CanvasPositionChangedEvent(Vector delta);
        public event CanvasPositionChangedEvent CanvasPositionChanged;

        private Point _startPoint;
        private bool _isDragging;

        private FrameworkElement element;

        public void Attach(FrameworkElement element)
        {
            var AssociatedObject = this.element = element;
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeave += OnMouseLeave;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(Canvas);
            element.CaptureMouse();
            e.Handled = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            element.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var currentPoint = e.GetPosition(Canvas);
            if (currentPoint.X < 10)
                currentPoint.X = 10;
            if (currentPoint.Y < 10)
                currentPoint.Y = 10;

            var diff = currentPoint - _startPoint;


            _startPoint = currentPoint;


            CanvasPositionChanged?.Invoke(diff);
            e.Handled = true;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            element.ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}