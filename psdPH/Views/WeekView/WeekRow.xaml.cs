using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для WeekRow.xaml
    /// </summary>
    public partial class WeekRow : UserControl
    {
        public WeekRow(WeekConfig weekGaleryConfig, WeekData data)    
        {
            InitializeComponent();
            rowStack.Children.Add(new WeekTile(data));
            foreach (KeyValuePair<DayOfWeek, Blob> item in data.DayBlobsDict)
                rowStack.Children.Add(new DayTile(weekGaleryConfig, item.Key, data.DayBlobsDict[item.Key]));
        }
    }
}
