using System.Windows;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для DateFormatWindow.xaml
    /// </summary>

    public partial class DateFormatWindow : Window
    {
        public DateFormatWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, "Z");
        }
    }
}
