using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace psdPH
{
    public class DrawingGroupToImage
    {
 
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cropper cropper = new Cropper(new Size(300,500));
        public MainWindow()
        {
            InitializeComponent();
            addFlagItem.CommandParameter = "flag";

            //border_semitrans.Opacity = 0.5;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cropper.ShowDialog();
        }

        private void CalendarDayButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DropdownMenu.IsOpen = true;

        }
        public void msg(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("flag");
        }
        static class AD { 
            
        }
    }
}
