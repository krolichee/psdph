using psdPH.Logic.Compositions;
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
        public WeekRow(WeekConfig weekConfig, WeekData data)    
        {
            InitializeComponent();
            rowStack.Children.Add(new WeekTile(data, weekConfig));
            foreach (KeyValuePair<DayOfWeek, Blob> item in data.DowBlobsDict)
                rowStack.Children.Add(new DayTile(weekConfig, item.Key, data.DowBlobsDict[item.Key]));
        }
    }
}
