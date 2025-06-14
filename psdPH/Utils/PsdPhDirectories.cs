﻿using System.IO;
using Path = System.IO.Path;


namespace psdPH
{
    public static class PsdPhDirectories
    {
        public static void SetBaseDirectory(string path)
        {
            //Directory.SetCurrentDirectory(@"C:");
            var v = Directory.CreateDirectory(Path.GetFullPath(path));
            //Directory.SetCurrentDirectory(path);
            BaseDirectory = path;
        }
        public static string ProjectPsd(string projectName) => Path.Combine(PsdPhDirectories.ProjectDirectory(projectName), "template.psd");
        public static string ProjectXml(string projectName) => Path.Combine(PsdPhDirectories.ProjectDirectory(projectName), "template.xml");
        private static string CreateIfNotExist(string path)
        {
            var V = Directory.CreateDirectory(path);
            return V.FullName;
        }
        public static string BaseDirectory;
        public static string ProjectsDirectory => CreateIfNotExist(Path.Combine(BaseDirectory, "Projects"));

        public static string CollectionsDirectory => Path.Combine(BaseDirectory, "Collections");

        public static string ProjectDirectory(string projectName) => Path.Combine(ProjectsDirectory, projectName);
        public static string ViewsDirectory(string projectName) => CreateIfNotExist(Path.Combine(ProjectDirectory(projectName), "Views"));
    }
}
