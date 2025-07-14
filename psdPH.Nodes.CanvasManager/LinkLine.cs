using psdPH.Nodes.UI;
using psdPH.Nodes.UI.UI;
using psdPH.Setups;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private static readonly Dictionary<Line, LinkLineEffect> _effects = new Dictionary<Line, LinkLineEffect>();

        public static void Clear(Line line) => _effects.Remove(line);

        public static void PaintDefault(Line line)
        {
            if (!_effects.TryGetValue(line, out var effect))
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
            }
        }

        public static Line Create(NodeUI source, NodeUI dest, Canvas canvas)
        {
            var fromPoint = source.TransformToVisual(canvas).Transform(new Point(source.ActualWidth, 0));
            var toPoint = dest.TransformToVisual(canvas).Transform(new Point(0, 0));
            return CreateLine(fromPoint, toPoint, LinkLineEffect.Flow,
                () => dest.Node.Unchain(new NodeSetup(source.Node, Setup.None)));
        }

        public static Line Create(ChainBar chainBar, NodeUI nodeUI, Canvas canvas)
        {
            var fromPoint = chainBar.TransformToVisual(canvas).Transform(
                new Point(chainBar.ActualWidth, chainBar.ActualHeight / 2));
            var toPoint = nodeUI.TransformToVisual(canvas).Transform(new Point(0, 0));
            return CreateLine(fromPoint, toPoint, LinkLineEffect.Flow,
                () => nodeUI.Node.Unchain(chainBar.NodeSetup));
        }

        public static Line Create(SetupBar fromBar, SetupBar toBar, Canvas canvas)
        {
            var fromPoint = fromBar.TransformToVisual(canvas).Transform(
                new Point(fromBar.ActualWidth, fromBar.ActualHeight / 2));
            var toPoint = toBar.TransformToVisual(canvas).Transform(
                new Point(0, toBar.ActualHeight / 2));
            return CreateLine(fromPoint, toPoint, LinkLineEffect.None,
                () => Node.Unlink(fromBar.NodeSetup, toBar.NodeSetup));
        }

        private static Line CreateLine(Point fromPoint, Point toPoint, LinkLineEffect initialEffect, System.Action deleteAction)
        {
            var line = new Line
            {
                X1 = fromPoint.X,
                Y1 = fromPoint.Y,
                X2 = toPoint.X,
                Y2 = toPoint.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            _effects.Add(line, initialEffect);
            Paint(line, initialEffect);

            void HandleMouseEnter(object sender, MouseEventArgs e) => Paint(line, LinkLineEffect.Selected);
            void HandleMouseLeave(object sender, MouseEventArgs e) => PaintDefault(line);
            void HandleRightButtonDown(object sender, MouseEventArgs e) => deleteAction();

            line.MouseEnter += HandleMouseEnter;
            line.MouseLeave += HandleMouseLeave;
            line.MouseRightButtonDown += HandleRightButtonDown;

            return line;
        }
    }
}