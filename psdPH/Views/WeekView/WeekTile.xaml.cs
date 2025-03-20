using psdPH.Logic.Compositions;
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
        Composition[] exclude = new Composition[0];
        Blob blob;
        Composition[] getExcludes(WeekConfig weekConfig, Blob blob)
        {
            return new Composition[] {
                weekConfig.GetWeekDatesTextLeaf(blob)
            };

        }
        public WeekTile(WeekData data,WeekConfig weekConfig)
        {
            blob = data.MainBlob;
            exclude = getExcludes(weekConfig, blob);
            InitializeComponent();
            weekDateLabel.Content = weekConfig.GetWeekDatesTextLeaf(blob).Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DataInputWindow(blob, exclude).ShowDialog();
        }
    }
}
