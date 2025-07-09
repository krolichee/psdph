using psdPH.Nodes.Core;
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

namespace psdPH.Nodes.UI.UI
{
    /// <summary>
    /// Логика взаимодействия для ChainBar.xaml
    /// </summary>
    public partial class ChainBar : UserControl
    {
        public event NodeSetupEvent ChainPick;
        public readonly NodeSetup NodeSetup;
        public readonly NodeUI NodeUI;
        public ChainBar(NodeSetup nodeSetup,NodeUI nodeUI)
        {
            InitializeComponent();
            NodeUI = nodeUI;
            NodeSetup = nodeSetup;
            nameLabel.Content = NodeSetup.Setup.Config.Desc;
        }

        private void pickBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChainPick?.Invoke(NodeSetup);
            e.Handled = true;
        }
    }
}
