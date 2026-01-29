using System.Windows;
using System.Windows.Media;

namespace SimpleBrushBinding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Простое изменение цвета по клику
            myControl.MyBrush = myControl.MyBrush == Brushes.LightGray
            ? Brushes.Red
                : Brushes.LightGray;
        }
    }
}