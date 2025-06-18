using Photoshop;
using psdPH.Logic;
using System;
using System.IO;
using System.Windows;


namespace psdPH
{
    public partial class MainWindow
    {
        public static class ProjectCreator
        {
            public static string New()
            {

                MessageBoxResult result;
                do
                {
                    result = MessageBox.Show("Откройте шаблонируемый файл в Photoshop, затем нажмите 'Ок'", "", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Cancel)
                        return null;
                } while (!PhotoshopWrapper.HasOpenDocuments());

                var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;



                if (result == MessageBoxResult.Cancel)
                    return null;
                var si_w = new StringInputWindow("Введите название нового проекта");
                if (si_w.ShowDialog() != true)
                    return null;
                var projectName = si_w.GetResultString();

                if (!tryCreateProject(projectName))
                    return null;
                

                if (doc.IsNonFile())
                    copyPsdBySaving(doc, projectName);
                else

                if (!doc.Saved)
                {
                    var dialogResult = MessageBox.Show("Документ имеет несохранённые изменения. Сохранить их в новом проекте?", "", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                        copyPsdBySaving(doc, projectName);
                    else if (dialogResult == MessageBoxResult.No)
                        copyPsdByCopying(doc, projectName);
                    else
                        return null;
                }
                else
                    copyPsdByCopying(doc, projectName);
                return projectName;
            }
            static void copyPsdByCopying(Document doc, string projectName)
            {
                var filePath = doc.GetDocPath();
                string destinationPath = PsdPhDirectories.ProjectPsd(projectName);
                try
                {
                    File.Copy(filePath, destinationPath, overwrite: true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
                }
            }
            static void copyPsdBySaving(Document doc, string projectName)
            {
                doc.SaveDocument(PsdPhDirectories.ProjectPsd(projectName));
            }
            static bool tryCreateProject(string projectName)
            {
                string projectDirectory = PsdPhDirectories.ProjectDirectory(projectName);
                if (Directory.Exists(projectDirectory))
                {
                    MessageBox.Show("Такой проект уже существует");
                    return false;
                }
                Directory.CreateDirectory(projectDirectory);
                Directory.CreateDirectory(PsdPhDirectories.ViewsDirectory(projectName));
                return true;
            }
        }
    }
}
