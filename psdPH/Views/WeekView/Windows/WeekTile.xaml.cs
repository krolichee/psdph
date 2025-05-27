using psdPH.Logic.Compositions;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekTile.xaml
    /// </summary>
    public partial class WeekTile : UserControl
    {
        Composition[] exclude = new Composition[0];
        Blob blob;
        Composition[] getExcludes(WeekConfig weekConfig, Blob blob) =>
            new Composition[] {
                weekConfig.GetWeekDatesTextLeaf(blob)
            };

        public WeekTile(WeekData data, WeekConfig weekConfig)
        {
            blob = data.MainBlob;
            exclude = getExcludes(weekConfig, blob);
            InitializeComponent();
            weekDateLabel.Content = WeekFormat.getShortWeekDatesString(data.Week);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(blob, exclude).ShowDialog();
        }
    }
}
