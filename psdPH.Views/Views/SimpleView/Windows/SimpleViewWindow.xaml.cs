using psdPH.Views.SimpleView.Logic;
using System.Windows;
using psdPH.Utils.CedStack;
using System.ComponentModel;
using psdPH.Views.SimpleView.Windows.SimpleViewCedStack;
using psdPH.Project;
using psdPH.Photoshop;




namespace psdPH.Views.SimpleView.Windows
{
    /// <summary>
    /// Логика взаимодействия для SimpleViewWindow.xaml
    /// </summary>
    public partial class SimpleViewWindow : Window
    {
        SimpleView SimpleView = SimpleView.Instance() as SimpleView;
        private bool _doSave = true;
        private bool _deleted;
        public SimpleListData SimpleListData;
        DocumentWr doc;
        public bool Deleted { private set { _deleted = value; } get => _deleted; }
        public SimpleViewWindow(SimpleListData simpleListData)
        {
            var root = simpleListData.RootBlob;
            InitializeComponent();
            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new SimpleViewHandler(simpleListData)));
            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName)).Wrapper();
            SimpleListData = simpleListData;
        }
        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = SimpleView.Instance();
            weekView.Delete();
            _doSave = false;
            Close();
        }

        void save()
        {
            if (_doSave)
                SimpleView.Save();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            save();
        }
        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
