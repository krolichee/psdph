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


namespace psdPH
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        //Dictionary<string,Type>
        CropperWindow cropper = new CropperWindow(new System.Windows.Size(300, 500));
        void OpenProject(string path)
        {
            Directory.SetCurrentDirectory(path);
        }
        public MainWindow()
        {
            CompositionXmlDictionary.InitializeDictionary();
            OpenProject("C:\\Users\\Puziko\\source\\repos\\psdPH\\psdPH\\Projects\\test");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("template.xml");
            XmlNode root = xmlDoc.LastChild;
            if (root.Name != CompositionXmlDictionary.GetXmlName(typeof(Blob)))
                throw new Exception();

            string filePath = "template.psd";
            string absolutePath = System.IO.Path.GetFullPath(filePath);

            InitializeComponent();

            var config = new BlobEditorConfig(new Blob(filePath, BlobMode.Path));
            BlobEditorWindow editor = BlobEditorWindow.OpenFromDisk(config);
            editor.ShowDialog();
        }
        void test_ps(string absolutePath)
        {

            try
            {
                // Создаем экземпляр Photoshop
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                dynamic psApp = Activator.CreateInstance(psType);

                // Делаем Photoshop видимым
                psApp.Visible = true;

                // Открываем PSD файл
                dynamic doc = psApp.Open(absolutePath);

                // Получаем имена слоев и их типы
                GetLayerNamesAndTypes(doc);
                doc.SaveAs(@"C:\\Users\\Puziko\\Desktop\\p\\output.png", new PNGSaveOptions(), true, PsExtensionType.psLowercase);

                // Закрываем документ без сохранения
                doc.Close(PsSaveOptions.psDoNotSaveChanges);


                // Закрываем Photoshop
                System.Runtime.InteropServices.Marshal.ReleaseComObject(psApp);
                psApp = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void GetLayerNamesAndTypes(dynamic doc)
        {
            // Обрабатываем обычные слои (ArtLayers)
            foreach (dynamic layer in doc.ArtLayers)
            {
                Console.WriteLine($"Слой: {layer.Name}, Тип: {(PsLayerKind)layer.Kind}");
            }

            // Обрабатываем группы слоев (LayerSets)
            foreach (dynamic layerSet in doc.LayerSets)
            {
                Console.WriteLine($"Группа слоев: {layerSet.Name}, Тип: LayerSet");
                ProcessLayerSet(layerSet); // Рекурсивно обрабатываем вложенные слои
            }
        }

        static void ProcessLayerSet(dynamic layerSet)
        {
            // Обрабатываем обычные слои внутри группы
            foreach (dynamic layer in layerSet.ArtLayers)
            {
                Console.WriteLine($"Слой: {layer.Name}, Тип: {(PsLayerKind)layer.Kind} (в группе {layerSet.Name})");
            }

            // Обрабатываем вложенные группы слоев
            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                Console.WriteLine($"Группа слоев: {nestedLayerSet.Name}, Тип: LayerSet (в группе {layerSet.Name})");
                ProcessLayerSet(nestedLayerSet); // Рекурсивный вызов для вложенных групп
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cropper.ShowDialog();
        }

        private void CalendarDayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DropdownMenu.IsOpen = true;

        }
    }
}
