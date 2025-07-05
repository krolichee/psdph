using psdPH.Nodes.UI.UI;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using psdPH.Localization;

namespace psdPH.Nodes.UI
{
    /// <summary>
    /// Логика взаимодействия для NodeUI.xaml
    /// </summary>
    public partial class NodeUI : UserControl
    {
        public delegate void SetupBarEvent(SetupBar sb);
        public event SetupBarEvent SetupBarEnter;
        public event SetupBarEvent SetupBarLeave;

        

        Node _node;
        List<SetupBar> _setupBars = new List<SetupBar>();
        public SetupBar[] SetupBars => _setupBars.ToArray();
        public Node Node => _node;
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(
                "MyBrush",
                typeof(Brush),
                typeof(NodeUI),
                new PropertyMetadata(Brushes.LightGray));

        public Brush MyBrush
        {
            get => (Brush)GetValue(MyBrushProperty);
            set => SetValue(MyBrushProperty, value);
        }
        private SetupBar newSetupBar(Setup setup)
        {
            var nodeSetupLink = new NodeSetup(_node, setup);
            var setupBar = new SetupBar(nodeSetupLink, this);
            _setupBars.Add(setupBar);
            return setupBar;
        }
        public NodeUI(Node node)
        {
            _node = node;
            InitializeComponent();
            headLabel.Content = Localization.LocalizationService.Localize(node);
            MyBrush = NodeBrushes.GetBrush(node.GetType());
            foreach (var setup in node.Inputs)
                inputsStack.Children.Add(newSetupBar(setup));
            foreach (var setup in node.Outputs)
                outputsStack.Children.Add(newSetupBar(setup));
            foreach (var sb in _setupBars)
            {
                sb.MouseEnter += (_,__) => SetupBarEnter?.Invoke(sb);
                sb.MouseLeave += (_,__) => SetupBarLeave?.Invoke(sb);
            }
        }

        public event Action CanvasDragged;

        Point CanvasPosition
        {
            get => new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            set
            {
                CanvasDragged?.Invoke();
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
            }
        }

        private static Point _startPoint;
        private static bool _isDragging;
        private void headGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(null);
            ((UIElement)sender).CaptureMouse();
            e.Handled = true;
        }

        private void headGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ((UIElement)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void headGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var currentPoint = e.GetPosition(null);
            var diff = currentPoint - _startPoint;

            CanvasPosition += diff;
            _startPoint = currentPoint;
            e.Handled = true;
        }

        private void headGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            ((UIElement)sender).ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}
