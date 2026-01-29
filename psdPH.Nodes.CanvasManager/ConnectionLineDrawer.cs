using psdPH.Nodes.UI;
using psdPH.Nodes.UI.UI;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public enum LinkLineEffect
    {
        Simple,
        Selected,
        Bad,
        Flow
    }

    public static class ConnectionLineDrawer
    {
        public static Line CreateSequenceLine(NodeUI source, NodeUI dest, Canvas canvas)
        {
            var fromPoint = source.TransformToVisual(canvas).Transform(new Point(source.ActualWidth, 0));
            var toPoint = dest.TransformToVisual(canvas).Transform(new Point(0, 0));
            var line = CreateLine(fromPoint, toPoint);
            Action deleteAction = () => dest.Node.Unchain(new NodeSetup(source.Node, Setup.None));

            ConnectionLinkLineModel.Register(line, deleteAction, LinkLineEffect.Flow);
            return line;
        }
        public static Line CreateChainLine(ChainBar chainBar, NodeUI nodeUI, Canvas canvas)
        {
            var fromPoint = chainBar.TransformToVisual(canvas).Transform(
                new Point(chainBar.ActualWidth, chainBar.ActualHeight / 2));
            var toPoint = nodeUI.TransformToVisual(canvas).Transform(new Point(0, 0));
            Action deleteAction = () => nodeUI.Node.Unchain(chainBar.NodeSetup);

            var line = CreateLine(fromPoint, toPoint);
            ConnectionLinkLineModel.Register(line, deleteAction, LinkLineEffect.Flow);
            return line;

        }
        public static Line CreateLinkLine(SetupBar fromBar, SetupBar toBar, Canvas canvas)
        {
            var fromPoint = fromBar.TransformToVisual(canvas).Transform(
                new Point(fromBar.ActualWidth, fromBar.ActualHeight / 2));
            var toPoint = toBar.TransformToVisual(canvas).Transform(
                new Point(0, toBar.ActualHeight / 2));
            Action deleteAction = () => Node.Unlink(fromBar.NodeSetup, toBar.NodeSetup);
            var line = CreateLine(fromPoint, toPoint);
                ConnectionLinkLineModel.Register(line, deleteAction, LinkLineEffect.Simple);
            return line;
        }


        public static void Paint(Line line, LinkLineEffect effect)
        {
            switch (effect)
            {
                case LinkLineEffect.Simple:
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






        private static Line CreateLine(Point fromPoint, Point toPoint)
        {
            var line = new Line
            {
                X1 = fromPoint.X,
                Y1 = fromPoint.Y,
                X2 = toPoint.X,
                Y2 = toPoint.Y
            };
            return line;
        }
    }
}