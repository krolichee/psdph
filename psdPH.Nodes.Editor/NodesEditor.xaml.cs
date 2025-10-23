using psdPH.Localization;
using psdPH.Logic.Compositions;
using psdPH.Nodes.CanvasManager;
using psdPH.Nodes.Nodes;
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

namespace psdPH.Nodes.Editor
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class NodesEditor : UserControl
    {
        Composition blob;

        public static readonly DependencyProperty ObjectBundleProperty =
        DependencyProperty.Register(
            nameof(NodesEditor.StructureBundle),
            typeof(Dictionary<string, Action>),
            typeof(NodesEditor),
            new PropertyMetadata(null));
        public Dictionary<string, Action> StructureBundle
        {
            get => (Dictionary<string, Action>)GetValue(ObjectBundleProperty);
            set => SetValue(ObjectBundleProperty, value);
        }
        public static readonly DependencyProperty ParameterBundleProperty =
        DependencyProperty.Register(
            nameof(NodesEditor.ParameterBundle),
            typeof(Dictionary<string, Action>),
            typeof(NodesEditor),
            new PropertyMetadata(null));
        public Dictionary<string, Action> ParameterBundle
        {
            get => (Dictionary<string, Action>)GetValue(ParameterBundleProperty);
            set => SetValue(ParameterBundleProperty, value);
        }
        public class HeaderActionModel
        {
            public string Header { get; set; }
            public ICommand Command { get; set; }
            public object CommandParameter { get; set; }
        }
        public static readonly DependencyProperty CalcBundleProperty =
        DependencyProperty.Register(
            nameof(NodesEditor.CalcBundle),
            typeof(HeaderActionModel[]),
            typeof(NodesEditor),
            new PropertyMetadata(null));
        public HeaderActionModel[] CalcBundle
        {
            get => (HeaderActionModel[])GetValue(CalcBundleProperty);
            set => SetValue(CalcBundleProperty, value);
        }

        public void UpdateCalcBundle()
        {
            var result = new List<HeaderActionModel>();
            var nodes = new Node[] {
                new MuxNode(),
                new ForkNode()
            };

            foreach (var node in nodes)
            {
                var ham = new HeaderActionModel()
                {
                    Header = LocalizationService.Localize((node as object).GetType()),
                    Command = new RelayCommand((_node) => NodeCanvasManager.Instance().AddNodeToModel(_node as Node)),
                    CommandParameter = node
                };
                result.Add(ham);
            }
            CalcBundle = result.ToArray();
        }

        // Метод для обновления словаря (вызывается при открытии контекстного меню)
        public void UpdateObjectBundle()
        {
            var result = new Dictionary<string, Action>();
            foreach (var item in blob.Children)
            {
                var caption = LocalizationService.Localize((item as object).GetType());
                Action creationCommand = () => NodeCanvasManager.Instance().AddNodeToModel(new ObjectNode(item));
                result.Add(caption, creationCommand);
            }
            StructureBundle = result;
        }
        private void UIElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            UpdateObjectBundle();
            UpdateCalcBundle();
        }

        public NodesEditor(Composition blob)
        {
            this.blob = blob;
            this.DataContext = this;
            UpdateObjectBundle();
            UpdateCalcBundle();
            InitializeComponent();
            var canvas = NodeCanvasManager.MakeInstance(blob).Canvas;
            PanManager.Attach(canvas, canvasViewer);
            canvasBorder.Child= canvas;
            
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            UpdateObjectBundle();
            UpdateCalcBundle();
        }
    }
}
