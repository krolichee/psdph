using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace psdPH.Nodes.CanvasManager
{
    public static class CanvasExtension
    {
        public static  Point GetCanvasPoint(this FrameworkElement e)
        {
            return new Point(Canvas.GetLeft(e), Canvas.GetTop(e));
        }
        public static  void SetCanvasPoint(this FrameworkElement e, Point point)
        {
            Canvas.SetLeft(e, point.X);
            Canvas.SetTop(e, point.Y);
        }

        public static Rect GetCanvasRect(this FrameworkElement e)
        {
            return new Rect(GetCanvasPoint(e), e.RenderSize);
        }
        public static void SetCanvasRect(this FrameworkElement e, Rect rect)
        {
            SetCanvasPoint(e, rect.TopLeft);
            e.Width = rect.Width;
            e.Height = rect.Height;
        }
    }
}
