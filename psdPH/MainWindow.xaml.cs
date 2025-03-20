using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Photoshop;
using System.Xml.Linq;
using psdPH.TemplateEditor;
using System.Xml;
using System.Collections;
using psdPH.Logic;
using System.Xml.Serialization;
using Path = System.IO.Path;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System.Runtime.Remoting.Metadata;
using System.Linq.Expressions;
using Condition = psdPH.Logic.Rules.Condition;
using static psdPH.WeekViewWindow;
using PhotoshopTypeLibrary;
using psdPH.Logic.Compositions;
using System.IO.Pipes;
using psdPH.Views.WeekView;
using psdPH.Utils;


namespace psdPH
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static double K_PixToWidth = 0.7488;

        public static double K_WidthToPix = 0.7488;
        static class Directories
        {
            public static string BaseDirectory;
            public static string ProjectsDirectory => Path.Combine(BaseDirectory, "Projects");
            public static string ProjectDirectory => Path.Combine(ProjectsDirectory, MainWindow.CurrentProjectName);
            public static string ViewsDirectory => Path.Combine(ProjectDirectory, "Views");
        }
        public static string GetFieldName<T>(Expression<Func<T>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
        public static string CurrentProjectName;
        //Dictionary<string,Type>
        CropperWindow cropper = new CropperWindow(new System.Windows.Size(300, 500));
        void OpenProject(string projectName)
        {
            CurrentProjectName = projectName;
            projectNameLabel.Content = CurrentProjectName;
        }
        void NewProject()
        {
            new PsdTemplateDropWindow().ShowDialog();
        }
        public void InitializeMiscellanious()
        {
            Directories.BaseDirectory = Directory.GetCurrentDirectory();
        }
        public MainWindow()
        {
           // Получаем типы из сборки
            //var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            //var doc = psApp.ActiveDocument;
            //Console.WriteLine((doc.ActiveLayer as ArtLayer).GetBoundsSize());
            //doc.FitTextLayer("textLayer", "Пн");
            //ArtLayer layer = doc.ActiveLayer;
            //layer.TextItem.Width = 100;
            //Console.WriteLine(layer.TextItem.Width);
            //layer.TextItem.Height = 100;
            //Console.WriteLine(layer.TextItem.Height);
            //layer.TextItem.Size -= 10;
            InitializeMiscellanious();
            InitializeComponent();
            LoadFoldersIntoMenu();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cropper.ShowDialog();
        }
        private void LoadFoldersIntoMenu()
        {
            string directoryPath = Directories.ProjectsDirectory; // Укажите путь к директории

                string[] folders = Directory.GetDirectories(directoryPath);
                foreach (string folder in folders)
                {
                    MenuItem folderMenuItem = new MenuItem
                    {
                        Header = Path.GetFileName(folder)
                    };
                    folderMenuItem.Click += FolderMenuItem_Click;
                    openMenuItem.Items.Add(folderMenuItem);
                }

        }
        private void FolderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            if (clickedMenuItem != null)
            {
                string folderName = clickedMenuItem.Header.ToString();
                OpenProject(folderName);
            }
        }

        private void NewProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewProject();
        }
        
        private void templateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Blob blob = openMainBlob();
            BlobEditorCfg bec = new BlobEditorCfg() { Composition = blob };
            ICompositionEditor editor = bec.Factory.CreateCompositionEditorWindow(null, bec, blob);
            editor.ShowDialog();
            saveBlob(blob);
        }
        

        private void weekkViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Blob blob = openMainBlob();
            WeekConfig weekConfig = null;
            WeekListData weeksListData = null;

            string configPath = Path.Combine(Directories.ViewsDirectory, "WeekView", "config.xml");
            weekConfig = DiskOperations.openXml<WeekConfig>(configPath);

            string dataPath = Path.Combine(Directories.ViewsDirectory, "WeekView", "data.xml");
            weeksListData = DiskOperations.openXml<WeekListData>(dataPath);
            if (weeksListData != null)
            {
                weeksListData.Restore();
                weeksListData.RootBlob = blob;
            }

            var wv_w = new WeekViewWindow(blob, weekConfig, weeksListData);
            if (wv_w.ShowDialog() == true)
            {
                weekConfig = wv_w.WeekConfig;
                weeksListData = wv_w.WeekListData;
                DiskOperations.saveXml(configPath, weekConfig);
                DiskOperations.saveXml(dataPath, weeksListData);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            PhotoshopWrapper.Dispose();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadFoldersIntoMenu();
        }
    }
}
