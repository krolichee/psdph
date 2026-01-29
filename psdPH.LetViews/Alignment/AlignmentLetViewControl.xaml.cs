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

namespace psdPH.LetViews
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AlignmentLetViewControl : UserControl
    {
        public AlignmentLetViewControl(Let let)
        {
            InitializeComponent();
            DataContext = new CaptionLetViewModel(let);
        }
    }
}
