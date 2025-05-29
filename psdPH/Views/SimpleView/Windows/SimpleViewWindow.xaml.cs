using psdPH.Views.SimpleView.Logic;
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
using System.Windows.Shapes;
using psdPH.Views.SimpleView.Logic;
using psdPH.Utils.CedStack;
using psdPH.Views.WeekView;
using System.ComponentModel;
using psdPH.Views.SimpleView.Windows.SimpleViewCedStack;
using Photoshop;



namespace psdPH.Views.SimpleView.Windows
{
    /// <summary>
    /// Логика взаимодействия для SimpleViewWindow.xaml
    /// </summary>
    public partial class SimpleViewWindow : Window
    {
        private bool _deleted;
        public SimpleListData SimpleListData;
        Document doc;
        public bool Deleted { private set { _deleted = value; } get => _deleted; }
        public SimpleViewWindow(SimpleListData simpleListData)
        {
            var root = simpleListData.RootBlob;
            InitializeComponent();
            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new SimpleViewHandler(simpleListData)));
            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            SimpleListData = simpleListData;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = psdPH.Views.SimpleView.Logic.SimpleView.Instance();
            weekView.Delete();
            Deleted = true;
            Close();
        }
    }
}
