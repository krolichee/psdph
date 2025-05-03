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
        private bool _deleted;
        public bool Deleted { private set { _deleted = value; } get => _deleted; }
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
            doc = PhotoshopWrapper.OpenDocument(psApp, Directories.ProjectPsd(PsdPhProject.Instance().ProjectName));

            if (weekListData == null)
            {
                weekListData = WeekListData.Create(weekConfig, root);
            }
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            WeekListData = weekListData;
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Delete();
            Deleted = true;
            Close();
        }
    }
}
