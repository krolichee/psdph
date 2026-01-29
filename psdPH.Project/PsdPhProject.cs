using psdPH.Logic.Compositions;
using psdPH.Utils;
using System.IO;
using System.Windows;

namespace psdPH.Project
{
    public class PsdPhProject
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
            Directory.CreateDirectory(PsdPhDirectories.ProjectDirectory(projectName));
        }
        public void saveBlob(RootBlob blob) => saveBlob(blob, ProjectName);
        public RootBlob openOrCreateMainBlob() => openOrCreateMainBlob(ProjectName);
        public RootBlob createMainBlob() => createMainBlob(ProjectName);
        public RootBlob openMainBlob() => openMainBlob(ProjectName);

        public static void saveBlob(RootBlob blob, string projectName)
        {
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            var result = DiskOperations.SaveXml(xmlFilePath, blob);
            if (!(result.Serialized && result.Written))
                MessageBox.Show("Во время сохранения произошла ошибка",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public RootBlob openOrCreateMainBlob(string projectName)
        {
            RootBlob blob;
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            if (File.Exists(xmlFilePath))
            {
                blob = openMainBlob(projectName);
            }
            else
                blob = createMainBlob(projectName);

            return blob;
        }
        static RootBlob createMainBlob(string projectName)
        {
            return new RootBlob();
        }
        RootBlob suggestCreateDefaultBlob(RootBlob blob)
        {
            var dialogResult = MessageBox.Show("Заменить на пустой шаблон?", "Ошибка открытия", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
                blob = createMainBlob();
            return blob;
        }
        RootBlob openMainBlob(string projectName)
        {
            RootBlob blob = null;
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            try
            {
                blob = DiskOperations.LoadXml<RootBlob>(xmlFilePath);
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
