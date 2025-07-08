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



        private readonly Node _node;
        private readonly List<SetupBar> _setupBars = new List<SetupBar>();
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
        private SetupBar NewSetupBar(Setup setup)
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
            var cdBehavior = new CanvasDragBehavior();
            cdBehavior.Attach(headGrid);
            cdBehavior.CanvasPositionChanged += CdBehavior_CanvasPositionChanged;
            MyBrush = NodeBrushes.GetBrush(node.GetType());
            foreach (var setup in node.Inputs)
                inputsStack.Children.Add(NewSetupBar(setup));
            foreach (var setup in node.Outputs)
                outputsStack.Children.Add(NewSetupBar(setup));
            foreach (var sb in _setupBars)
            {
                sb.MouseEnter += (_,__) => SetupBarEnter?.Invoke(sb);
                sb.MouseLeave += (_,__) => SetupBarLeave?.Invoke(sb);
            }
        }

        private void CdBehavior_CanvasPositionChanged(Vector delta)
        {
            CanvasPosition += delta;
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
    }
}
