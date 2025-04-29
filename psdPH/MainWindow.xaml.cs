using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;


namespace psdPH
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static string CurrentProjectName = "";
        void OpenProject(string projectName)
        {
            CurrentProjectName = projectName;
            projectNameLabel.Content = CurrentProjectName;
        }
        void CloseProject(object _)
        {
            CurrentProjectName = "";
        }
        bool tryCreateProject(string templatePath,string projectName)
        {
            string projectDirectory = Path.Combine(Directories.ProjectsDirectory, projectName);
            if (Directory.Exists(projectDirectory))
            {
                MessageBox.Show("Такой проект уже существует");
                return false;
            }
            Directory.CreateDirectory(projectDirectory);
            string destinationPath = Path.Combine(projectDirectory, "template.psd");
            try
            {
                // Копируем файл в целевую директорию
                File.Copy(templatePath, destinationPath, overwrite: true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
                return false;
            }
        }
        void NewProject()
        {
            MessageBoxResult result;
            do
            {
                result = MessageBox.Show("Откройте шаблонируемый файл в Photoshop, затем нажмите 'Ок'", "", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Cancel)
                    return;
            } while (!PhotoshopWrapper.HasOpenDocuments());
            if (result == MessageBoxResult.Cancel)
                return;
            var si_w = new StringInputWindow("Введите название нового проекта");
            if (si_w.ShowDialog() != true)
                return;
            var filePath = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument.FullName;
            MessageBox.Show(filePath);
            tryCreateProject(filePath,si_w.getResultString());
            LoadFoldersIntoMenu();
        }
        public void InitializeBaseDirectory()
        {
            string path = Path.Combine("C","ProgramData","psdPH");
            Directory.CreateDirectory(path);
            Directories.BaseDirectory = path; //Directory.GetCurrentDirectory();
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
            InitializeBaseDirectory();
            InitializeComponent();
            LoadFoldersIntoMenu();
            templateMenuItem.Command = new RelayCommand(templateMenuItem_Click, isProjectOpen);
            weekkViewMenuItem.Command = new RelayCommand(weekkViewMenuItem_Click, isProjectOpen);
            openMenuItem.Command = new RelayCommand(noneCommand, isAnyProject);
            closeProjectMenuItem.Command = new RelayCommand(CloseProject, isProjectOpen);
        }
        private void noneCommand(object _) { }
        private bool isAnyProject(object _)
        {
           return getProjectsFolders().Any();
        }
        private string[] getProjectsFolders()
        {
            string directoryPath = Directories.ProjectsDirectory;
            return Directory.GetDirectories(directoryPath);
        }
        private MenuItem makeOpenProjectMenuItem(string folder)
        {
            MenuItem folderMenuItem = new MenuItem();
            folderMenuItem.Header = Path.GetFileName(folder);
            folderMenuItem.Click += FolderMenuItem_Click;
            return folderMenuItem;
        }
        private void LoadFoldersIntoMenu()
        {
            LoadFoldersIntoMenu(getProjectsFolders());
        }
        private void LoadFoldersIntoMenu(string[] folders)
        {
            MenuItem[] items = folders.Select(makeOpenProjectMenuItem).ToArray();
            openMenuItem.ItemsSource = items;
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
        bool isProjectOpen(object _)
        {
            return CurrentProjectName != "";
        }
        private void templateMenuItem_Click(object sender)
        {
            Blob blob = PsdPhProject.openOrCreateMainBlob(CurrentProjectName);
            ICompositionShapitor editor = BlobEditorWindow.OpenFromDisk(blob);
            editor.ShowDialog();
            PsdPhProject.saveBlob(blob, CurrentProjectName);
        }
        private void weekkViewMenuItem_Click(object _)
        {
            Blob blob = PsdPhProject.openOrCreateMainBlob(CurrentProjectName);
            WeekConfig weekConfig = null;
            WeekListData weeksListData = null;

            string configPath = Path.Combine(Directories.ViewsDirectory(CurrentProjectName), "WeekView", "config.xml");
            weekConfig = DiskOperations.openXml<WeekConfig>(configPath);

            string dataPath = Path.Combine(Directories.ViewsDirectory(CurrentProjectName), "WeekView", "data.xml");
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
