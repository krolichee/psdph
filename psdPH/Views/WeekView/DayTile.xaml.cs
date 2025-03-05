using Photoshop;
using psdPH.Logic;
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
    /// Логика взаимодействия для DayTile.xaml
    /// </summary>
    public partial class DayTile : UserControl
    {
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
        public DayTile(WeekConfig weekGaleryConfig, DayOfWeek key,Blob blob)
        {
            InitializeComponent();
            this.WeekGaleryConfig = weekGaleryConfig;
            this.Key = key;
            this.blob = blob;

            refreshPreview();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(blob).ShowDialog();
            refreshPreview();
        }
    }
}
