using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace psdPH.Nodes.UI
{
        public static class LinkLine
        {
        public enum LinkLineEffect
        {
            None,
            Selected,
            Bad
        }
            public static void Paint(Line line, LinkLineEffect effect) {
            switch (effect)
            {
                case LinkLineEffect.None:
                    line.StrokeThickness = 1;
                    line.Stroke = Brushes.Black;
                    break;
                case LinkLineEffect.Selected:
                    line.StrokeThickness = 4;
                    line.Stroke = new SolidColorBrush(Colors.Cyan);
                    break;
                case LinkLineEffect.Bad:
                    line.StrokeThickness = 2;
                    line.Stroke = new SolidColorBrush(Colors.Red);
                    break;
                default:
                    break;
            }
        }
            public static Line Create(SetupBar fromBar, SetupBar toBar, Canvas canvas)
            {
                Point fromSetupBarPoint = new Point(fromBar.ActualWidth, fromBar.ActualHeight / 2);
                Point fromSetupBarPointInNodeUI = fromBar.TransformToVisual(fromBar.NodeUI).Transform(fromSetupBarPoint);
                Point fromSetupBarPointInCanvas = fromBar.NodeUI.TransformToVisual(canvas).Transform(fromSetupBarPointInNodeUI);

                Point toSetupBarPoint = new Point(0, toBar.ActualHeight / 2);
                Point toSetupBarPointInNodeUI = toBar.TransformToVisual(toBar.NodeUI).Transform(toSetupBarPoint);
                Point toSetupBarPointInCanvas = toBar.NodeUI.TransformToVisual(canvas).Transform(toSetupBarPointInNodeUI);

                var line = new Line()
                {
                    X1 = fromSetupBarPointInCanvas.X,
                    Y1 = fromSetupBarPointInCanvas.Y,
                    X2 = toSetupBarPointInCanvas.X,
                    Y2 = toSetupBarPointInCanvas.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                void Line_MouseLeave(object sender, MouseEventArgs e)
                {
                Paint(line, LinkLineEffect.None);
                }

                void Line_MouseEnter(object sender, MouseEventArgs e)
                {
                Paint(line, LinkLineEffect.Selected);
            }

                line.MouseRightButtonDown += deleteLine;

                void deleteLine(object _, MouseEventArgs e)
                {
                    fromBar.NodeSetup.Node.Unlink(fromBar.NodeSetup, toBar.NodeSetup);
                }
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                return line;
            }
        
    }
}
