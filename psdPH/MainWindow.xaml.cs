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
using Condition =psdPH.Logic.Rules.Condition;
using static psdPH.WeekViewWindow;


namespace psdPH
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
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

        public MainWindow()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(List<KeyValuePair< DayOfWeek, string >>));
            //var obj = new Dictionary<DayOfWeek, string>() { { DayOfWeek.Friday, "dasd" }, { DayOfWeek.Monday, "dasd" } };
            //var obj_list = obj.ToList();
            //FileStream fs = new FileStream("test.xml", FileMode.Create);
            //serializer.Serialize(fs, obj.ToList());
            //var dsr_obj_list = new List<KeyValuePair<DayOfWeek, string>>();
            //fs.Close();
            //fs = new FileStream("test.xml", FileMode.Open);
            //dsr_obj_list = (List<KeyValuePair<DayOfWeek, string>>)serializer.Deserialize(fs);
            //var dsr_obj = dsr_obj_list.ToDictionary(pair => pair.Key, pair => pair.Value); ;
            Directories.BaseDirectory = Directory.GetCurrentDirectory();
            CompositionXmlDictionary.InitializeDictionary();
            InitializeComponent();
            LoadFoldersIntoMenu();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cropper.ShowDialog();
        }

        private void CalendarDayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadFoldersIntoMenu()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Projects"); // Укажите путь к директории

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    // Получаем все подпапки в указанной директории
                    string[] folders = Directory.GetDirectories(directoryPath);

                    foreach (string folder in folders)
                    {
                        // Создаем новый MenuItem для каждой папки
                        MenuItem folderMenuItem = new MenuItem
                        {
                            Header = Path.GetFileName(folder)
                        };

                        // Добавляем обработчик события для клика по элементу меню
                        folderMenuItem.Click += FolderMenuItem_Click;

                        // Добавляем MenuItem в главное меню
                        openMenuItem.Items.Add(folderMenuItem);
                    }
                }
                else
                {
                    MessageBox.Show("Указанная директория не существует.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке папок: {ex.Message}");
            }
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }
        private void FolderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Обработка клика по элементу меню
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
        private void saveBlob(Blob blob)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Composition));
            string xmlFilePath = Path.Combine(Directories.ProjectDirectory, "template.xml");
            FileStream writeFileStream = new FileStream(xmlFilePath, FileMode.Create);
            serializer.Serialize(writeFileStream, blob);
            writeFileStream.Close();
        }
        private Blob openMainBlob()
        {
            Blob blob;
            XmlSerializer serializer = new XmlSerializer(typeof(Composition));
            string xmlFilePath = Path.Combine(Directories.ProjectDirectory, "template.xml");
            string psdFilePath = Path.Combine(Directories.ProjectDirectory, "template.psd");
            if (File.Exists(xmlFilePath))
            {
                //if (rootNode.Name != CompositionXmlDictionary.GetXmlName(typeof(Blob)))
                //    throw new Exception();


                FileStream readFileStream = new FileStream(xmlFilePath, FileMode.Open);

                blob = serializer.Deserialize(readFileStream) as Blob;
                readFileStream.Close();
                blob.Restore();
            }
            else
                blob = Blob.PathBlob(psdFilePath);
            return blob;
        }

        private void templateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Blob blob = openMainBlob();
            BlobEditorConfig bec = new BlobEditorConfig() { Composition = blob };
            ICompositionEditor editor = bec.Factory.CreateCompositionEditorWindow(null, bec, blob);
            editor.ShowDialog();
            saveBlob(blob);
        }

        private void weekkViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Blob blob = openMainBlob();
            WeekConfig weekConfig = null;
            WeekListData weeksListData = null;
            XmlSerializer configSerializer = new XmlSerializer(typeof(WeekConfig));
            XmlSerializer dataSerializer = new XmlSerializer(typeof(WeekListData));
            FileStream fileStream;
            string configPath = Path.Combine(Directories.ViewsDirectory, "WeekView", "config.xml");
            if (File.Exists(configPath))
            {
                fileStream = new FileStream(configPath, FileMode.Open);
                weekConfig = (WeekConfig)configSerializer.Deserialize(fileStream);
                //weekDowsConfig.Restore(blob);
                fileStream.Close();
            }
            string dataPath = Path.Combine(Directories.ViewsDirectory, "WeekView", "data.xml");
            if (File.Exists(dataPath))
            {
                fileStream = new FileStream(dataPath, FileMode.Open);
                weeksListData = (WeekListData)dataSerializer.Deserialize(fileStream);
                weeksListData.Restore();
                weeksListData.RootBlob = blob;
                //weekDowsConfig.Restore(blob);
                fileStream.Close();
            }
            var wgv_w = new WeekViewWindow(blob, weekConfig, weeksListData);
            wgv_w.ShowDialog();
            {
                fileStream = new FileStream(configPath, FileMode.Create);
                weekConfig = wgv_w.WeekDowsConfig;
                configSerializer.Serialize(fileStream, weekConfig);
                fileStream.Close();
            }
            {
                fileStream = new FileStream(dataPath, FileMode.Create);
                weeksListData = wgv_w.WeekListData;
                dataSerializer.Serialize(fileStream, weeksListData);
                fileStream.Close();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
