using psdPH.Nodes.UI;
using psdPH.Nodes.UI.UI;
using psdPH.Setups;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public enum LinkLineEffect
    {
        None,
        Selected,
        Bad,
        Flow
    }
    public static class LinkLine
    {
        public static void Clear(Line line) {
            effects.Remove(line);
        }
        static Dictionary<Line, LinkLineEffect> effects = new Dictionary<Line, LinkLineEffect>();
        public static void PaintDefault(Line line) {
            LinkLineEffect effect;
            if (!effects.TryGetValue(line, out effect))
                effect = LinkLineEffect.None;
            Paint(line, effect); 
        }
        public static void Paint(Line line, LinkLineEffect effect)
        {
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
                case LinkLineEffect.Flow:
                    line.StrokeThickness = 2;
                    line.Stroke = new SolidColorBrush(Colors.Gold);
                    break;
                default:
                    break;
            }
        }
        public static Line Create(NodeUI source, NodeUI dest, Canvas canvas)
        {
            Point fromNodeUIPoint = new Point(source.ActualWidth, 0);
            Point fromNodeUIPointInCanvas = source.TransformToVisual(canvas).Transform(fromNodeUIPoint);

            Point toNodeUIPoint = new Point(0, 0);
            Point toNodeUIPointInCanvas = dest.TransformToVisual(canvas).Transform(toNodeUIPoint);

            var line = new Line()
            {
                X1 = fromNodeUIPointInCanvas.X,
                Y1 = fromNodeUIPointInCanvas.Y,
                X2 = toNodeUIPointInCanvas.X,
                Y2 = toNodeUIPointInCanvas.Y
            };
            effects.Add(line, LinkLineEffect.Flow);
            PaintDefault(line);
            void Line_MouseLeave(object sender, MouseEventArgs e)
            {
                PaintDefault(line);
            }

            void Line_MouseEnter(object sender, MouseEventArgs e)
            {
                Paint(line, LinkLineEffect.Selected);
            }

            line.MouseRightButtonDown += deleteLine;

            void deleteLine(object _, MouseEventArgs e)
            {
                dest.Node.Unchain(new NodeSetup(dest.Node, Setup.None));
            }
            line.MouseEnter += Line_MouseEnter;
            line.MouseLeave += Line_MouseLeave;
            return line;
        }
        public static Line Create(ChainBar chainBar, NodeUI nodeUI, Canvas canvas)
        {
            Point fromChainBarPoint = new Point(chainBar.ActualWidth, chainBar.ActualHeight / 2);
            Point fromChainBarPointInNodeUI = chainBar.TransformToVisual(chainBar.NodeUI).Transform(fromChainBarPoint);
            Point fromChainBarPointInCanvas = chainBar.NodeUI.TransformToVisual(canvas).Transform(fromChainBarPointInNodeUI);

            Point toNodeUIPoint = new Point(0, 0);
            Point toNodeUIPointInCanvas = nodeUI.TransformToVisual(canvas).Transform(toNodeUIPoint);

            var line = new Line()
            {
                X1 = fromChainBarPointInCanvas.X,
                Y1 = fromChainBarPointInCanvas.Y,
                X2 = toNodeUIPointInCanvas.X,
                Y2 = toNodeUIPointInCanvas.Y,
            };
            effects.Add(line, LinkLineEffect.Flow);
            PaintDefault(line);
            void Line_MouseLeave(object sender, MouseEventArgs e)
            {
                PaintDefault(line);
            }

            void Line_MouseEnter(object sender, MouseEventArgs e)
            {
                Paint(line, LinkLineEffect.Selected);
            }

            line.MouseRightButtonDown += deleteLine;

            void deleteLine(object _, MouseEventArgs e)
            {
                nodeUI.Node.Unchain(chainBar.NodeSetup);
            }
            line.MouseEnter += Line_MouseEnter;
            line.MouseLeave += Line_MouseLeave;
            return line;
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
            effects.Add(line, LinkLineEffect.None);
            void Line_MouseLeave(object sender, MouseEventArgs e)
            {
                PaintDefault(line);
            }

            void Line_MouseEnter(object sender, MouseEventArgs e)
            {
                Paint(line, LinkLineEffect.Selected);
            }

            line.MouseRightButtonDown += deleteLine;

            void deleteLine(object _, MouseEventArgs e)
            {
                Node.Unlink(fromBar.NodeSetup, toBar.NodeSetup);
            }
            line.MouseEnter += Line_MouseEnter;
            line.MouseLeave += Line_MouseLeave;
            return line;
        }

    }
}
