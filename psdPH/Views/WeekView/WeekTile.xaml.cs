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

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для WeekTile.xaml
    /// </summary>
    public partial class WeekTile : UserControl
    {
        Blob blob;
        public WeekTile(WeekData data)
        {
            blob = data.MainBlob;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(blob).ShowDialog();
        }
    }
}
