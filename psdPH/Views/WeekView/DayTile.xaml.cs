using psdPH.Logic;
using psdPH.Logic.Compositions;
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
        Composition[] exclude = new Composition[0];
        Blob blob;
        private WeekConfig weekGaleryConfig;
        public WeekConfig WeekGaleryConfig
        {
            get => weekGaleryConfig;
            set
            {
                weekGaleryConfig = value;
            }
        }

        public DayOfWeek Key
        {
            get => key; set
            {
                key = value;
                dowLabel.Content = Key.GetDescription();
            }
        }
        private DayOfWeek key;

        void refreshPreview()
        {
            previewTextBlock.Text = blob.getChildren<TextLeaf>().First(t => t.LayerName == WeekGaleryConfig.TilePreviewTextLeafName).Text;
        }
        Composition[] getExcludes(WeekConfig weekConfig, Blob blob)
        {
            return new Composition[] {
                weekConfig.GetDowTextLeaf(blob),
                weekConfig.GetDateTextLeaf(blob),
            };

        }
        public DayTile(WeekConfig weekConfig, DayOfWeek key, Blob blob)
        {
            InitializeComponent();
            this.WeekGaleryConfig = weekConfig;
            this.Key = key;
            this.blob = blob;
            exclude = getExcludes(weekConfig, blob);
            refreshPreview();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(blob, exclude).ShowDialog();
            refreshPreview();
        }
    }
}
