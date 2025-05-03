using psdPH.Logic.Compositions;
using psdPH.Utils;
using System.IO;

namespace psdPH
{
    internal class PsdPhProject
    {
        public static void saveBlob(Blob blob, string projectName)
        {
            string xmlFilePath = Directories.ProjectXml(projectName);
            DiskOperations.SaveXml<Blob>(xmlFilePath, blob);
        }
        public static Blob openOrCreateMainBlob(string projectName)
        {
            Blob blob;
            string xmlFilePath = Directories.ProjectXml(projectName);
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
            string psdFilePath = Directories.ProjectPsd(projectName);
            return Blob.PathBlob(psdFilePath);
        }
        public static Blob openMainBlob(string projectName)
        {
            Blob blob;
            string xmlFilePath = Directories.ProjectXml(projectName);
            blob = DiskOperations.OpenXml<Blob>(xmlFilePath);
            blob.Restore();
            return blob;
        }
    }
}
