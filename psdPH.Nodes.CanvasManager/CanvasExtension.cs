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
        public static  Point getCanvasPoint(this FrameworkElement e)
        {
            return new Point(Canvas.GetLeft(e), Canvas.GetTop(e));
        }
        public static  void setCanvasPoint(this FrameworkElement e, Point point)
        {
            Canvas.SetLeft(e, point.X);
            Canvas.SetTop(e, point.Y);
        }

        public static Rect getCanvasRect(this FrameworkElement e)
        {
            return new Rect(getCanvasPoint(e), e.RenderSize);
        }
        public static void setCanvasRect(this FrameworkElement e, Rect rect)
        {
            setCanvasPoint(e, rect.TopLeft);
            e.Width = rect.Width;
            e.Height = rect.Height;
        }
    }
}
