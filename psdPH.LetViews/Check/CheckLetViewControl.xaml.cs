using psdPH.Lets;
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

namespace psdPH.LetViews.Check
{
    /// <summary>
    /// Interaction logic for CheckLetViewControl.xaml
    /// </summary>
    public partial class CheckLetViewControl : UserControl
    {
        public CheckLetViewControl(Let let)
        {
            InitializeComponent();
            DataContext = new CaptionLetViewModel(let);
        }
    }
}
