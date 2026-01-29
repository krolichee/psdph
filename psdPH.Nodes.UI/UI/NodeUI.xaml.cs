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
using System.Windows.Media.Effects;
using psdPH.Lets;

namespace psdPH.Nodes.UI
{
    /// <summary>
    /// Логика взаимодействия для NodeUI.xaml
    /// </summary>
    public partial class NodeUI : UserControl
    {
        public readonly CanvasDragBehavior CanvasDragBehavior;

        public delegate void SetupBarEvent(SetupBar sb);
        

        public event SetupBarEvent SetupBarEnter;
        public event SetupBarEvent SetupBarLeave;

        public event NodeEvent ChainPut;
        public event NodeEvent ChainPick;

        public event Action<FrameworkElement,Vector> CanvasDragged;


        private readonly Node _node;
        private readonly List<SetupBar> _setupBars = new List<SetupBar>();
        private readonly List<ChainBar> _chainBars = new List<ChainBar>();
        public SetupBar[] SetupBars => _setupBars.ToArray();
        public ChainBar[] ChainBars => _chainBars.ToArray();
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
        private SetupBar NewLetBar(Let Let)
        {
            var nodeLet = new NodeLet(_node, Let);
            var setupBar = new SetupBar(nodeLet, this);
            _setupBars.Add(setupBar);
            return setupBar;
        }
        public NodeUI(Node node)
        {
            _node = node;
            InitializeComponent();
            headLabel.Text = Localization.LocalizationService.Localize(node);
            CanvasDragBehavior = new CanvasDragBehavior();

            CanvasDragBehavior.Attach(headGrid);
            CanvasDragBehavior.CanvasPositionChanged += CdBehavior_CanvasPositionChanged;
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
            foreach (var chain in node.Chains)
            {
                chainsStack.Children.Add(NewChainBar(chain));
            }
        }
        public bool Selected
        {
            set { 
                Opacity = value ? 0.5 : 1;
                //contentGrid.BitmapEffect = value ? new DropShadowBitmapEffect() {Softness = 0,ShadowDepth = 0,Opacity = 0.5,Color = Colors.AliceBlue} :null;
            }
        }
        private void CdBehavior_CanvasPositionChanged(Vector delta)
        {
            CanvasPosition += delta;
            CanvasDragged?.Invoke(this,delta);
        }

        Point CanvasPosition
        {
            get => new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            set
            {
                
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
            }
        }

        private void pickBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChainPick?.Invoke(Node);
            e.Handled = true;
        }

        private void headGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChainPut?.Invoke(Node);
        }
    }
}
