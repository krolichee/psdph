using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System.IO;

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
            DiskOperations.SaveXml<Blob>(xmlFilePath, blob);
        }
        public static Blob openOrCreateMainBlob(string projectName)
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
        public static Blob createMainBlob(string projectName)
        {
            string psdFilePath = PsdPhDirectories.ProjectPsd(projectName);
            return Blob.PathBlob(Path.GetFileName(psdFilePath));
        }
        public static Blob openMainBlob(string projectName)
        {
            Blob blob;
            string xmlFilePath = PsdPhDirectories.ProjectXml(projectName);
            blob = DiskOperations.OpenXml<Blob>(xmlFilePath);
            blob.Restore();
            return blob;
        }

    }
}
