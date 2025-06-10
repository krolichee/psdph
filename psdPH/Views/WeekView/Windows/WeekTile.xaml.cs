using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekTile.xaml
    /// </summary>
    public partial class WeekTile : UserControl
    {
        ParameterSet Parset;
        public WeekTile(WeekData weekData)
        {
            Parset = weekData.ParameterSet;
            InitializeComponent();
            weekDateLabel.Content = WeekFormat.getShortWeekDatesString(weekData.Week);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ParsetInputWindow(Parset).ShowDialog();
        }
    }
}
