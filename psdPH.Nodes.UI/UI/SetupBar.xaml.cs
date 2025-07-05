using psdPH.Setups;
using psdPH.Utils.Setups;
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
    /// Логика взаимодействия для SetupBar.xaml
    /// </summary>
    public partial class SetupBar : UserControl
    {
        public readonly NodeUI NodeUI;
        public readonly NodeSetup NodeSetup;
        public SetupBar(NodeSetup nodeSetupLink, NodeUI nodeUI)
        {
            NodeUI= nodeUI;
            NodeSetup = nodeSetupLink;
            var setup = NodeSetup.Setup;
            var control = setup.Control;
            setup.Stack.Children.Remove(control );
            InitializeComponent();
            Grid.SetColumn(control,1);
            control.HorizontalAlignment = HorizontalAlignment.Center;
            control.VerticalAlignment = VerticalAlignment.Center;
            setupControlBorder.Child =control;
            nameLabel.Content = setup.Config.Desc;
        }

        private void mainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NodeConvasManager.Instance().LinkDraggedTo(NodeSetup);
            Console.WriteLine("Перетащено значение ноды");
        }

        private void pickBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NodeConvasManager.Instance().DraggedLink = NodeSetup;
            Console.WriteLine("Захват значение ноды");
        }

        private void mainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            NodeConvasManager.Instance().PreviewLink(NodeSetup);
        }
    }
}
