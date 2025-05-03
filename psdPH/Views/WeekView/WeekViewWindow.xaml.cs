using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils.CedStack;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public partial class WeekViewWindow : Window
    {
        public WeekConfig WeekConfig=>WeekListData.WeekConfig;
        public WeekListData WeekListData;
        Document doc;
        public WeekViewWindow(WeekListData weekListData)
        {
            var root = weekListData.RootBlob;
            var weekConfig = weekListData.WeekConfig;
            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekStackHandler()));
            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, root.Path);
            
            if (weekListData == null)
            {
                weekListData = WeekListData.Create(weekConfig, root);
            }
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            WeekListData = weekListData;
            InitializeComponent();
        }
    }
}
