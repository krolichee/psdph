using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;

namespace psdPHTest
{
	[TestClass]
	public class ProjectTestSuite
	{
        protected PsdPhProject project;
        protected string basePath;
        protected string ProjectName => project.ProjectName;
        [TestInitialize]
        public void Init()
        {
            var projectName = "test";
            basePath = Path.GetTempFileName().Replace(".tmp", "");
            PsdPhDirectories.SetBaseDirectory(basePath);
            project = PsdPhProject.MakeInstance(projectName);
        }
        [TestCleanup]
        public void Korin()
        {
            Directory.Delete(basePath, true);
        }
    }
}
