using Photoshop;
using psdPH.Utils.CedStack;
using System.ComponentModel;
using System.Windows;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public partial class WeekViewWindow : Window
    {
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        public WeekListData WeekListData;
        Document doc;
        public WeekViewWindow(WeekListData weekListData)
        {
            var root = weekListData.RootBlob;
            var weekConfig = weekListData.WeekConfig;
            InitializeComponent();
            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekStackHandler(weekListData)));
            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, root.Path);

            if (weekListData == null)
            {
                weekListData = WeekListData.Create(weekConfig, root);
            }
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            WeekListData = weekListData;
            
        }
    }
}
