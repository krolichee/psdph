using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using psdPH.Views.SimpleView;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.SimpleView.Windows;
using psdPH.Views.WeekView;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;


namespace psdPH
{
    public partial class MainWindow : Window
    {
        Window _templateWindow;
        Window _viewWindow;
        Window TemplateWindow
        {
            get => _templateWindow; set
            {
                if (value != null)
                    value.Closed += (object _, EventArgs __) => TemplateWindow = null;
                IsEnabled = (_templateWindow = value) == null;
            }
        }
        Window ViewWindow
        {
            get => _viewWindow; set
            {
                if (value != null)
                    value.Closed += (object _, EventArgs __) => ViewWindow = null;
                IsEnabled = (_viewWindow = value) == null;
            }
        }
        public static string CurrentProjectName = "";
        void OpenProject(string projectName)
        {
            PsdPhProject.MakeInstance(projectName);
            CurrentProjectName = projectName;
            projectNameTextBlock.Text = CurrentProjectName;
        }
        void CloseProject_Execute(object _)
        {
            CurrentProjectName = "";
            projectNameTextBlock.Text = CurrentProjectName;
        }

        
        bool AnyViews()
        {
            return Directory.EnumerateFileSystemEntries(PsdPhDirectories.ViewsDirectory(CurrentProjectName)).Any();
        }
        void NewProject()
        {
            var projectName = ProjectCreator.New();
            if (projectName == null)
                return;
            OpenProject(projectName);
            LoadFoldersIntoMenu();
        }
        
        public string BaseDirectory;
        public void InitializeBaseDirectory()
        {
            PsdPhDirectories.SetBaseDirectory(BaseDirectory); //Directory.GetCurrentDirectory();
        }
        
        public void ExportExamples()
        {
            //if (Directory.Exists(BaseDirectory))
            //    return;
            Directory.CreateDirectory(BaseDirectory);
            var examplesDir = Path.Combine(Directory.GetCurrentDirectory(), "Examples");
            var targerDir = PsdPhDirectories.ProjectsDirectory;
            DiskOperations.CopyDirectory(examplesDir, targerDir);
        }
        public MainWindow() : this(Path.Combine(@"C:\", "ProgramData", "psdPH")) { }
        MainWindow(string baseDirectory)
        {

            BaseDirectory = baseDirectory;
            InitializeComponent();

            InitializeBaseDirectory();
            ExportExamples();

            LoadFoldersIntoMenu();
            InitializeButtonCommands();


        }
        void InitializeButtonCommands()
        {
            RelayCommand projectOpenDepended(Action<object> action) => new RelayCommand(action, isProjectOpen);
            RelayCommand notDepended(Action<object> action) => new RelayCommand(action, (object _) => true);
            //Проект
            newProjectMenuItem.Command = notDepended(NewProjectMenuItem_Execute);
            openMenuItem.Command = new RelayCommand(noneCommand_Execute, isAnyProject);
            closeProjectMenuItem.Command = projectOpenDepended(CloseProject_Execute);
            openInExplorerMenuItem.Command = projectOpenDepended(openInExplorer_Execute);
            //Шаблон
            templateMenuItem.Command = projectOpenDepended(templateMenuItem_Execute);
            //Виды
            weekViewMenuItem.Command = projectOpenDepended(weekViewMenuItem_Execute);
            simpleViewMenuItem.Command = projectOpenDepended(simpleViewMenuItem_Execute);
        }
        private void openInExplorer_Execute(object _)
        {
            string folderPath = PsdPhDirectories.ProjectDirectory(CurrentProjectName);
            Process.Start("explorer.exe", folderPath);
        }
        private void noneCommand_Execute(object _) { }
        private bool isAnyProject(object _)
        {
            return getProjectsFolders().Any();
        }
        private string[] getProjectsFolders()
        {
            string directoryPath = PsdPhDirectories.ProjectsDirectory;
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

        private void NewProjectMenuItem_Execute(object _)
        {
            NewProject();
        }
        bool isProjectOpen(object _)
        {
            return CurrentProjectName != "";
        }
        private void templateMenuItem_Execute(object _)
        {
            //if (AnyViews())
            //{
            //    MessageBox.Show("Изменения шаблона не будут отображаться на уже созданных видах. После редактирования необходимо будет заново создавать либо очищать виды");
            //}
            TemplateWindow = BlobEditorWindow.OpenFromDisk();
        }
        private void weekViewMenuItem_Execute(object _)
        {
            ViewWindow = WeekView.ShowWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            PhotoshopWrapper.Dispose();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadFoldersIntoMenu();
        }

        private void simpleViewMenuItem_Execute(object _)
        {
            ViewWindow = SimpleView.ShowWindow();   
        }
    }
}
