using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.SimpleView.Windows;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Views.SimpleView.Logic
{
    public class SimpleView
    {
        private static SimpleView _instance;
        private readonly string _projectName;
        public static SimpleView Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static SimpleView MakeInstance(string projectName)
        {
            return _instance = new SimpleView(projectName);
        }
        protected SimpleView(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
        }

        private string ViewDirectory => Path.Combine(PsdPhDirectories.ViewsDirectory(_projectName), "SimpleView");
        private string SimpleListDataPath => Path.Combine(ViewDirectory, "data.xml");
        public string OutputsDirectory => Path.Combine(ViewDirectory, "output");
        public string OutputDirectory(string outputName) => Path.Combine(OutputsDirectory, outputName);
        public void CreateOutputsDirectory() => Directory.CreateDirectory(OutputsDirectory);
        public void CreateOutputDirectory(string outputName) => Directory.CreateDirectory(OutputDirectory(outputName));
        public SimpleListData OpenSimpleListData() => DiskOperations.OpenXml<SimpleListData>(SimpleListDataPath);

        public void SaveListData(SimpleListData simpleListData)
        {
            DiskOperations.SaveXml(SimpleListDataPath, simpleListData);
            PsdPhProject.Instance().saveBlob(simpleListData.RootBlob);
        }

        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
        }
        public static Window ShowWindow()
        {
            var project = PsdPhProject.Instance();
            var simpleView = MakeInstance(project.ProjectName);
            Blob blob = project.openOrCreateMainBlob();
            SimpleListData simpleListData;
            try
            {
                simpleListData = simpleView.OpenSimpleListData();
                if (simpleListData == null)
                    simpleListData = new SimpleListData(blob);
                else
                    simpleListData.Restore(blob);
            }
            catch
            {
                var result = MessageBox.Show("Во время открытия данных вида произошла ошибка. Удалить вид?", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                    Instance().Delete();
                return null;
            }
            

            if (simpleListData == null)
                return null;
            var window = new SimpleViewWindow(simpleListData);
            window.Show();
            return window;
        }

    }
}
