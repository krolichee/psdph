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
        public delegate void NodeLetEvent(NodeLet nodeLet);

        readonly NodeUI nodeUI;
        readonly NodeLet nodeLet;

        public event NodeLetEvent NodeSetupPick;
        public event NodeLetEvent NodeSetupHover;
        public event NodeLetEvent NodeSetupPut;

        public SetupBar(NodeUI nodeUI,NodeLet nodeLet)
        {
            this.nodeUI = nodeUI;
            this.nodeLet = nodeLet;
            var control = nodeLet.View;
            InitializeComponent();
            Grid.SetColumn(control, 1);
            control.HorizontalAlignment = HorizontalAlignment.Center;
            control.VerticalAlignment = VerticalAlignment.Center;
            setupControlBorder.Child = control;
            nameLabel.Content = nodeLet.Let.Name;
        }

        public bool SetupControlVisibility
        {
            set => setupControlBorder.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }


        private void mainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NodeSetupPut?.Invoke(nodeLet);
        }

        private void pickBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NodeSetupPick?.Invoke(nodeLet);
            e.Handled = true;
        }

        private void mainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            NodeSetupHover?.Invoke(nodeLet);
        }
    }
}
