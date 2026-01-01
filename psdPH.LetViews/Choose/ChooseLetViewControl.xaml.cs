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

namespace psdPH.LetViews.Choose
{
    /// <summary>
    /// Interaction logic for ChooseLetControl.xaml
    /// </summary>
    public partial class ChooseLetViewControl : UserControl
    {
        public ChooseLetViewControl(Let let, object[] options)
        {
            DataContext = new ChooseLetViewModel(let, options);
            InitializeComponent();
        }
    }
}
