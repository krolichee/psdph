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
        readonly Composition root;

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

        
        NodeCanvasManager()
        {
            var canvas = new Canvas() { Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                Width = 2000, Height=2000,
                HorizontalAlignment=HorizontalAlignment.Left,VerticalAlignment=VerticalAlignment.Top};
            Canvas = canvas;
            Canvas.Focusable = true;
            LinkPullManager.Attach(this);
            ChainPullManager.Attach(this);
            SelectionManager.Attach(this);
            
        }

        public NodeCanvasManager(Composition blob):this()
        {
            root = blob;
            LoadNodes(blob);

            UpdateLines();
        }

        private void LoadNodes(Composition blob)
        {
            foreach (var node in blob.NodeSet.Nodes)
                AddNodeToView(node);
        }

        private void UpdateLines()
        {
            List<NodeUI> nodeUIs = new List<NodeUI>();
            foreach (var item in Canvas.Children.OfType<UIElement>().ToList())
            {
                if (item is Line line)
                {
                    Canvas.Children.Remove(line);
                    ConnectionLinkLineModel.Clear(line);
                }
                if (item is NodeUI nodeUI)
                    nodeUIs.Add(nodeUI);
            }
            new SetupLinksDrawer(Canvas).DrawSetupLinks(nodeUIs);
            new ChainLinksDrawer(Canvas).DrawChainLinks(nodeUIs);
        }

        public void AddNodeToModel(Node node)
        {
            root.NodeSet.Nodes.Add(node);
            AddNodeToView(node);
        }

        void AddNodeToView(Node node)
        {

            var nodeUI = new NodeUI(node);
            NodeUIAdded?.Invoke(nodeUI);
            nodeUI.CanvasDragBehavior.Canvas = Canvas;
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
        public void DeleteElementFromModel(FrameworkElement[] items)
        {
            foreach (var item in items)
            if (item is Line line)
                DeleteLineFromModel(line); 
            else if (item is NodeUI nodeUI)
                DeleteNodeFromModel(nodeUI);
            UpdateLines();
        }
        private void DeleteNodeFromModel(NodeUI nodeUI)
        {
            Canvas.Children.Remove(nodeUI);
            root.NodeSet.Nodes.Remove(nodeUI.Node);
        }
        private void DeleteLineFromModel(Line line)
        {
            ConnectionLinkLineModel.Get(line).Delete();
            
        }
    }
}
