using psdPH.Lets;
using psdPH.Lets.Core;
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
    /// Interaction logic for LetBar.xaml
    /// </summary>
    public partial class LetBar : UserControl
    {
        public LetBar(LetView letView)
        {
            DataContext = new LetBarViewModel(letView);
            InitializeComponent();
        }
    }
}
