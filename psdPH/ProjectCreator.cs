using Photoshop;
using psdPH.Logic;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;


namespace psdPH
{
    public partial class MainWindow
    {
        public static class ProjectCreator
        {
            class StringContainer
            {
                public string Value { get; set; }
            }
            public static string New()
            {

                MessageBoxResult result;
                do
                {
                    result = MessageBox.Show("Откройте шаблонируемый файл в Photoshop, затем нажмите 'Ок'", "", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Cancel)
                        return null;
                } while (!PhotoshopWrapper.HasOpenDocuments());

                var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument.Wrapper();
                
                var tempObj = new StringContainer();


                var projectNameConfig = new ReflectionConfig(tempObj,nameof(tempObj.Value), "Название нового проекта");
                var projectNameSetup = new StringInputSetup(projectNameConfig);

                if (result == MessageBoxResult.Cancel)
                    return null;
                
                var si_w = new SetupsInputWindow(new[]{ projectNameSetup });
                if (si_w.ShowDialog() != true)
                    return null;
               
                var projectName = tempObj.Value;

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
            static void copyPsdByCopying(DocumentWr doc, string projectName)
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
            static void copyPsdBySaving(DocumentWr doc, string projectName)
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
