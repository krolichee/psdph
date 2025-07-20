using psdPH.Logic.Compositions;
using psdPH.Nodes.UI;
using psdPH.Nodes.UI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace psdPH.Nodes.CanvasManager
{
    public partial class NodeCanvasManager
    {
        public readonly Canvas Canvas;
        static NodeCanvasManager instance;
        public delegate void NodeUIEvent(NodeUI nodeUI);
        public event NodeUIEvent NodeUIAdded;
        readonly RootBlob root;

        public static NodeCanvasManager Instance()
        {
            if (instance == null)
                throw new Exception();
            return instance;
        }
        public static NodeCanvasManager MakeInstance(Composition blob)
        {
            instance = new NodeCanvasManager(blob);
            return Instance();
        }
        [Obsolete]
        public static NodeCanvasManager MakeInstance(Canvas canvas,ScrollViewer scrollViewer)
        {
            instance = new NodeCanvasManager(canvas,scrollViewer);
            return instance;
        }
        [Obsolete]
        NodeCanvasManager(Canvas canvas,ScrollViewer scrollViewer)
        {
            Canvas = canvas;
            LinkPullManager.Attach(this);
            ChainPullManager.Attach(this);
            SelectionManager.Attach(this);
            PanManager.Attach(canvas,scrollViewer);
            UpdateLines();

        }
        NodeCanvasManager()
        {
            var canvas = new Canvas() { Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                Width = 2000, Height=2000,
                HorizontalAlignment=HorizontalAlignment.Left,VerticalAlignment=VerticalAlignment.Top};
            Canvas = canvas;
            LinkPullManager.Attach(this);
            ChainPullManager.Attach(this);
            SelectionManager.Attach(this);
            
        }

        public NodeCanvasManager(Composition blob):this()
        {
            LoadNodes(blob);

            UpdateLines();
        }

        private void LoadNodes(Composition blob)
        {
            foreach (var node in blob.NodeSet.Nodes)
            {
                AddNode(node);
            }
            
        }

        private void UpdateLines()
        {
            List<NodeUI> nodeUIs = new List<NodeUI>();
            foreach (var item in Canvas.Children.OfType<UIElement>().ToList())
            {
                if (item is Line line)
                {
                    Canvas.Children.Remove(line);
                    LinkLine.Clear(line);
                }
                if (item is NodeUI nodeUI)
                    nodeUIs.Add(nodeUI);
            }
            new SetupLinksDrawer(Canvas).DrawSetupLinks(nodeUIs);
            new ChainLinksDrawer(Canvas).DrawChainLinks(nodeUIs);
        }

        

        public void AddNode(Node node)
        {

            var nodeUI = new NodeUI(node);
            NodeUIAdded?.Invoke(nodeUI);

            nodeUI.Node.Links.CollectionChanged += (_, __) => UpdateLines();
            nodeUI.Node.ChainChanged += UpdateLines;

            nodeUI.CanvasDragged += NodeUI_CanvasDragged;


            if (double.IsNaN(Canvas.GetLeft(nodeUI)))
                Canvas.SetLeft(nodeUI, 0);
            if (double.IsNaN(Canvas.GetTop(nodeUI)))
                Canvas.SetTop(nodeUI, 0);
            Canvas.Children.Add(nodeUI);

        }
        private void NodeUI_CanvasDragged(FrameworkElement sender, Vector delta)
        {
            var canvasRect = sender.GetCanvasRect();
            Canvas.Width = new[] { Canvas.Width, canvasRect.Right }.Max() ;
            Canvas.Height = new[] { Canvas.Height, canvasRect.Bottom }.Max() ;
            UpdateLines();
        }
    }
}
