using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Path = System.IO.Path;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    /// <summary>
    /// Логика взаимодействия для BlobPathEditorWindow.xaml
    /// </summary>
    public partial class BlobPathEditorWindow : Window,ICompositionEditor
    {
        // Приватные поля для хранения пути к файлу и имени
        private string _filePath;
        private string _fileName;
        Blob _blob;

        public BlobPathEditorWindow(BlobEditorConfig config)
        {
            _blob = config.Composition as Blob;
            _fileName = (config.Composition as Blob).Name;
            _filePath = (config.Composition as Blob).Path;
            InitializeComponent();
            // Настройка обработчиков событий для перетаскивания файла
            DropArea.Drop += DropArea_Drop;
            DropArea.DragEnter += DropArea_DragEnter;
            DropArea.DragOver += DropArea_DragOver;
            refresh();
        }
        private void refresh()
        {
            NameTextBox.Text = _fileName;

            
        }
        // Обработчик события нажатия кнопки "Выбрать файл"
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PSD Files (*.psd)|*.psd";

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                _fileName = NameTextBox.Text;
                MessageBox.Show($"Файл выбран: {_filePath}\nИмя: {_fileName}", "Успешно");
            }
        }

        // Обработчик события перетаскивания файла в область
        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && Path.GetExtension(files[0]).Equals(".psd", StringComparison.OrdinalIgnoreCase))
                {
                    _filePath = files[0];
                    _fileName = NameTextBox.Text;
                    MessageBox.Show($"Файл перетащен: {_filePath}\nИмя: {_fileName}", "Успешно");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, перетащите файл с расширением .psd", "Ошибка");
                }
            }
        }

        // Обработчик события входа файла в область перетаскивания
        private void DropArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // Обработчик события нахождения файла над областью перетаскивания
        private void DropArea_DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        public Composition getResultComposition()
        {
            _blob.Name = _fileName;
            _blob.Path = _filePath;
            return _blob;
        }
    }
}