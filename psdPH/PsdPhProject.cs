using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System;
using System.IO;
using System.Windows;

namespace psdPH
{
    internal class PsdPhProject
    {
        private static PsdPhProject _instance;
        public readonly string ProjectName;
        public static PsdPhProject Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static PsdPhProject MakeInstance(string projectName)
        {
            return _instance = new PsdPhProject(projectName);
        }
        protected PsdPhProject(string projectName)
        {
            ProjectName = projectName;
        }
        public void saveBlob(Blob blob) => saveBlob(blob, ProjectName);
        public Blob openOrCreateMainBlob() => openOrCreateMainBlob(ProjectName);
        public Blob createMainBlob() => createMainBlob(ProjectName);
        public Blob openMainBlob() => openMainBlob(ProjectName);

        public static void saveBlob(Blob blob, string projectName)
        {
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            var result = DiskOperations.SaveXml(xmlFilePath, blob);
            if (!(result.Serialized && result.Written))
                MessageBox.Show("Во время сохранения произошла ошибка",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public Blob openOrCreateMainBlob(string projectName)
        {
            Blob blob;
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            if (File.Exists(xmlFilePath))
            {
                blob = openMainBlob(projectName);
            }
            else
                blob = createMainBlob(projectName);

            return blob;
        }
        static Blob createMainBlob(string projectName)
        {
            string psdFilePath = PsdPhDirectories.ProjectPsd(projectName);
            return Blob.PathBlob(Path.GetFileName(psdFilePath));
        }
        Blob suggestCreateDefaultBlob(Blob blob)
        {
            var dialogResult = MessageBox.Show("Заменить на пустой шаблон?", "Ошибка открытия", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
                blob = createMainBlob();
            return blob;
        }
        Blob openMainBlob(string projectName)
        {
            Blob blob = null;
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            try
            {
                blob = DiskOperations.OpenXml<Blob>(xmlFilePath);
                if (blob.Mode != BlobMode.Path)
                {
                    MessageBox.Show("Это ошибка невероятной природы. Добиться её мог только тот, кто знает, что делает");
                    blob = suggestCreateDefaultBlob(blob);
                }
                blob.Restore();
            }
            catch { 
               MessageBox.Show("Не удалось открыть файл проекта. Возможно, он повреждён, либо принадлежит другой версии приложения",
                   "Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                blob = suggestCreateDefaultBlob(blob);
                
            }
            return blob;
        }

    }
}
