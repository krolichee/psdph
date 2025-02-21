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


namespace psdPH
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        string currentProject;
        //Dictionary<string,Type>
        CropperWindow cropper = new CropperWindow(new System.Windows.Size(300, 500));
        void OpenProject(string projectName)
        {
            Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(),"Projects",projectName));
            currentProject = projectName;
        }
        void NewProject()
        {
            new PsdTemplateDropWindow().ShowDialog();
        }

        public MainWindow()
        {
            CompositionXmlDictionary.InitializeDictionary();
            InitializeComponent();
            LoadFoldersIntoMenu();

        }
        void test_xml()
        {
            OpenProject("C:\\Users\\Puziko\\source\\repos\\psdPH\\psdPH\\Projects\\test");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("template.xml");
            XmlNode rootNode = xmlDoc.LastChild;
            if (rootNode.Name != CompositionXmlDictionary.GetXmlName(typeof(Blob)))
                throw new Exception();

            string filePath = "template.psd";
            string absolutePath = System.IO.Path.GetFullPath(filePath);

            
            Blob blob = new Blob(absolutePath, BlobMode.Path);
            blob.addChild(new Blob("asd", BlobMode.Layer));
            blob.addChild(new TextLeaf("someLayer"));
            var config = new BlobEditorConfig(blob);
            BlobEditorWindow editor = BlobEditorWindow.OpenFromDisk(config);
            editor.ShowDialog();
            XmlSerializer serializer = new XmlSerializer(typeof(Composition), CompositionXmlDictionary.StoT.Values.ToArray());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, blob);
            string xml = writer.ToString();
            Console.WriteLine("Сериализованный XML:");
            Console.WriteLine(xml);
            TextReader reader = new StringReader(xml);
            Blob deserializedObject = (Blob)serializer.Deserialize(reader);

            // Десериализуем XML обратно в объект
            //TextLeaf deserializedTextLeaf = Composition.DeserializeFromXml<TextLeaf>(xml);
            //Console.WriteLine($"Десериализованный объект: {deserializedTextLeaf.ObjName}");
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
            string directoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Projects"); // Укажите путь к директории

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
                            Header = System.IO.Path.GetFileName(folder)
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
                MessageBox.Show($"Вы выбрали папку: {folderName}");
                OpenProject(folderName);
            }
        }

        private void NewProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewProject();
        }

        private void templateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Blob), CompositionXmlDictionary.StoT.Values.ToArray());
            Blob blob;
            string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "template.xml");
            string psdFilePatg = Path.Combine(Directory.GetCurrentDirectory(), "template.psd");
            if (File.Exists(xmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("template.xml");
                XmlNode rootNode = xmlDoc.LastChild;
                if (rootNode.Name != CompositionXmlDictionary.GetXmlName(typeof(Blob)))
                    throw new Exception();



                FileStream readFileStream = new FileStream(Path.GetFullPath("template.xml"), FileMode.Open);

                blob = serializer.Deserialize(readFileStream) as Blob;
                readFileStream.Close();
                blob.restoreParents();
            }
            else
                blob = new Blob(psdFilePatg,BlobMode.Path);
            BlobEditorConfig bec = new BlobEditorConfig(blob);
            ICompositionEditor editor = bec.Factory.CreateCompositionEditorWindow(null, bec, blob);
            editor.ShowDialog();
            FileStream wrileFileStream = new FileStream(xmlFilePath, FileMode.Create);
            serializer.Serialize(wrileFileStream,blob);
            wrileFileStream.Close();
        }


        private void weekkViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Blob), CompositionXmlDictionary.StoT.Values.ToArray());
            Blob blob;
            string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "template.xml");
            string psdFilePatg = Path.Combine(Directory.GetCurrentDirectory(), "template.psd");
            if (File.Exists(xmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);
                XmlNode rootNode = xmlDoc.LastChild;
                if (rootNode.Name != CompositionXmlDictionary.GetXmlName(typeof(Blob)))
                    throw new Exception();



                FileStream readFileStream = new FileStream(xmlFilePath, FileMode.Open);

                blob = serializer.Deserialize(readFileStream) as Blob;
                readFileStream.Close();
                blob.restoreParents();
            }
            else
                blob = new Blob(psdFilePatg, BlobMode.Path);
            new WeekGaleryViewWindow(blob).ShowDialog();
        }
    }
}
