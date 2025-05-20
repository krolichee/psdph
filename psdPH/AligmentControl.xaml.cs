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
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для AligmentControl.xaml
    /// </summary>
    public partial class AligmentControl : UserControl
    {
        Size _size = new Size(90, 90);
        Alignment _result=new Alignment(HorizontalAlignment.Center,VerticalAlignment.Center);
        public AligmentControl()
        {
            InitializeComponent();
            var mainGrid = new Grid();

        }
        public Alignment GetResultAlignment()
        {
            return _result;
        }
    }
}
