using System;
using System.IO;
using System.Windows;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для PsdTemplateDropWindow.xaml
    /// </summary>
    public partial class PsdTemplateDropWindow : Window
    {
        private string filePath;

        public PsdTemplateDropWindow()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Получаем массив перетащенных файлов
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length > 0)
                {
                    filePath = files[0]; // Берем первый файл
                    labelDrop.Text = "Файл выбран";
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Projects", projectNameTB.Text);
            if (Directory.Exists(projectDirectory))
            {
                MessageBox.Show("Такой проект уже существует. Измените название");
                return;
            }

            Directory.CreateDirectory(projectDirectory);
            string destinationPath = Path.Combine(projectDirectory, "template.psd");
            try
            {
                // Копируем файл в целевую директорию
                File.Copy(filePath, destinationPath, overwrite: true);
                MessageBox.Show($"Проект успешно скопирован: {destinationPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
            }
            Close();
        }
    }
}
