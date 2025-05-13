using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.SimpleView.Logic
{
    class SimpleView
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

            private string ViewDirectory => Path.Combine(Directories.ViewsDirectory(_projectName), "SimpleView");
            private string ConfigPath => Path.Combine(ViewDirectory, "config.xml");
            private string WeekListDataPath => Path.Combine(ViewDirectory, "data.xml");
            public WeekConfig OpenWeekConfig() => DiskOperations.OpenXml<WeekConfig>(ConfigPath);
            public SimpleListData OpenSimpleListData() => DiskOperations.OpenXml<SimpleListData>(WeekListDataPath);
            public SimpleListData OpenOrCreateSimpleListData(Blob root)
            {
                var weeksListData = OpenSimpleListData();
                weeksListData.Restore();
                weeksListData.RootBlob = root;
                return weeksListData;
            }

            public void SaveWeekListData(SimpleListData simpleViewList)
            {
                DiskOperations.SaveXml(WeekListDataPath, simpleViewList);
            }

            internal void Delete()
            {
                Directory.Delete(ViewDirectory, true);
            }
    
    }
}
