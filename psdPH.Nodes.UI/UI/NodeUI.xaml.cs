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

namespace psdPH.Nodes.UI
{
    /// <summary>
    /// Логика взаимодействия для NodeUI.xaml
    /// </summary>
    public partial class NodeUI : UserControl
    {
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
        public SolidColorBrush SCB;
        public NodeUI(Node node)
        {
            
            InitializeComponent();
            headLabel.Content = (node as object).ToString();
            MyBrush = Brushes.Orchid;
            foreach (var setup in node.Inputs)
            {
                inputsStack.Children.Add(new SetupBar(setup));
            }
            foreach (var setup in node.Outputs)
            {
                outputsStack.Children.Add(new SetupBar(setup));
            }
        }
    }
}
