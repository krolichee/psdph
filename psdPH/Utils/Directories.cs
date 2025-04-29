using System.IO;
using Path = System.IO.Path;


namespace psdPH
{
        public static class Directories
        {
            public static string ProjectPsd(string projectName) => Path.Combine(Directories.ProjectDirectory(projectName), "template.psd");
        public static string ProjectXml(string projectName) => Path.Combine(Directories.ProjectDirectory(projectName), "template.xml");
        private static string CreateIfNotExist(string path)
            {
                return Directory.CreateDirectory(path).FullName;
            }
            public static string BaseDirectory;
            public static string ProjectsDirectory => CreateIfNotExist(Path.Combine(BaseDirectory, "Projects"));

        public static string CollectionsDirectory => Path.Combine(BaseDirectory, "Collections");

        public static string ProjectDirectory(string projectName) => Path.Combine(ProjectsDirectory, MainWindow.CurrentProjectName);
            public static string ViewsDirectory(string projectName) => CreateIfNotExist(Path.Combine(ProjectDirectory(projectName), "Views"));
        }
}
