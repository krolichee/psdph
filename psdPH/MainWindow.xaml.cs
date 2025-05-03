using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Views.WeekView;
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
            PsdPhProject.MakeInstance(projectName);
            CurrentProjectName = projectName;
            projectNameTextBlock.Text= CurrentProjectName;
        }
        void CloseProject(object _)
        {
            CurrentProjectName = "";
            projectNameTextBlock.Text = CurrentProjectName;
        }
        bool tryCreateProject(string templatePath, string projectName)
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
        bool AnyViews()
        {
           return Directory.EnumerateFileSystemEntries(Directories.ViewsDirectory(CurrentProjectName)).Any();
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
            var projectName = si_w.getResultString();
            if (tryCreateProject(filePath, projectName))
                OpenProject(projectName);
            Directory.CreateDirectory(Directories.ViewsDirectory(projectName));
            LoadFoldersIntoMenu();
        }
        public string BaseDirectory => Path.Combine(@"C:\", "ProgramData", "psdPH");
        public void InitializeBaseDirectory()
        {
            Directories.SetBaseDirectory(BaseDirectory); //Directory.GetCurrentDirectory();
        }
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            // Копируем все файлы
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, destFile, true); 
            }
            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string destDir = Path.Combine(targetDir, Path.GetFileName(directory));
                CopyDirectory(directory, destDir);
            }
        }
        public void ExportExamples()
        {
            //if (Directory.Exists(BaseDirectory))
            //    return;
            Directory.CreateDirectory(BaseDirectory);
            var examplesDir = Path.Combine(Directory.GetCurrentDirectory(),"Examples");
            var targerDir = Directories.ProjectsDirectory;
            CopyDirectory(examplesDir, targerDir);
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
            ExportExamples();
            InitializeComponent();
            LoadFoldersIntoMenu();
            templateMenuItem.Command = new RelayCommand(templateMenuItem_Click, isProjectOpen);
            weekkViewMenuItem.Command = new RelayCommand(weekViewMenuItem_Click, isProjectOpen);
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
            if(AnyViews())
            {
                MessageBox.Show("Изменения шаблона не будут отображаться на уже созданных видах. После редактирования необходимо будет заново создать виды");
            }    
            Blob blob = PsdPhProject.openOrCreateMainBlob(CurrentProjectName);
            ICompositionShapitor editor = BlobEditorWindow.OpenFromDisk(blob);
            editor.ShowDialog();
            PsdPhProject.saveBlob(editor.GetResultComposition() as Blob, CurrentProjectName);
        }
        private void weekViewMenuItem_Click(object _)
        {
            var weekView = WeekView.MakeInstance(CurrentProjectName);
            Blob blob = PsdPhProject.openOrCreateMainBlob(CurrentProjectName);
            var weekListData = weekView.OpenOrCreateWeekListData(blob);
            if (weekListData == null)
                return;
            var wv_w = new WeekViewWindow(weekListData);
            wv_w.ShowDialog();
            if (!wv_w.Deleted)
                weekView.SaveWeekListData(weekListData);
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
