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
        public SimpleListData SimpleListData;
        private static SimpleView _instance;
        private readonly string _projectName;
        public static SimpleView Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static SimpleView MakeInstance()
        {
            return _instance = new SimpleView(PsdPhProject.Instance().ProjectName);
        }
        protected SimpleView(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
            SimpleListData = tryOpenOrCreateData();
        }

        private string ViewDirectory => Path.Combine(PsdPhDirectories.ViewsDirectory(_projectName), "SimpleView");
        private string SimpleListDataPath => Path.Combine(ViewDirectory, "data.xml");
        public string OutputsDirectory => Path.Combine(ViewDirectory, "output");
        public string OutputDirectory(string outputName) => Path.Combine(OutputsDirectory, outputName);
        public void CreateOutputsDirectory() => Directory.CreateDirectory(OutputsDirectory);
        public void CreateOutputDirectory(string outputName) => Directory.CreateDirectory(OutputDirectory(outputName));
        public SimpleListData OpenData() => DiskOperations.OpenXml<SimpleListData>(SimpleListDataPath);

        public void Save()
        {
            DiskOperations.SaveXml(SimpleListDataPath, SimpleListData);
            PsdPhProject.Instance().saveBlob(SimpleListData.RootBlob);
        }

        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
        }

        public Window ShowWindow()
        {
            if (SimpleListData == null)
                return null;
            var window = new SimpleViewWindow(SimpleListData);
            window.Show();
            return window;
        }
        SimpleListData tryOpenOrCreateData()
        {
            try
            {
                return openOrCreateData();
            }
            catch
            {
                var result = MessageBox.Show("Во время открытия данных вида произошла ошибка. Удалить вид?", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                    Delete();
                return null;
            }
        }
        private SimpleListData openOrCreateData()
        {
            var project = PsdPhProject.Instance();
            Blob blob = project.openOrCreateMainBlob();
            SimpleListData simpleListData;
            simpleListData = OpenData();
            if (simpleListData == null)
                simpleListData = new SimpleListData(blob);
            else
                simpleListData.Restore(blob);
            return simpleListData;
        }

    }
}
