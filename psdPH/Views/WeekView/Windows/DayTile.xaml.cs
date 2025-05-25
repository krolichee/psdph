using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для DayTile.xaml
    /// </summary>
    public partial class DayTile : UserControl
    {
        public WeekData WeekData;
        WeekConfig _weekConfig => WeekData.WeekConfig;
        DowBlob _dayBlob => WeekData.DowBlobsDict[Dow];
        public DayOfWeek Dow
        {
            get => _dow; set
            {
                _dow = value;
                dowLabel.Content = Dow.GetDescription();
            }
        }
        private DayOfWeek _dow;

        void refreshPreview()
        {
            previewTextBlock.Text = WeekData.WeekConfig.GetTilePreviewTextLeaf(_dayBlob).Text;
        }

        Composition[] getExcludes()
        {
            return new Composition[] {
                _weekConfig.GetDowTextLeaf(_dayBlob),
                _weekConfig.GetDateTextLeaf(_dayBlob),
            };

        }
        public DayTile(WeekData weekData, DayOfWeek dow)
        {
            InitializeComponent();
            WeekData = weekData;
            this.Dow = dow;
            refreshPreview();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(_dayBlob, getExcludes()).ShowDialog();
            refreshPreview();
        }
    }
}
