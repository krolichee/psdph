using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.SimpleView.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views
{
    public abstract class ViewManager
    {
        protected static ViewManager _instance;
        protected string _projectName;
        protected void Initialize(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
        }
        public static ViewManager Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static ViewManager MakeInstance<T>(string projectName) where T: ViewManager, new()
        {
            var result = new T();
            result.Initialize(projectName);
            return _instance = result;
        }
        protected ViewManager()
        { 
        }
        protected virtual string ViewName { get; }
        private string ViewDirectory => Path.Combine(Directories.ViewsDirectory(_projectName), ViewName);
        private string ListDataPath => Path.Combine(ViewDirectory, "data.xml");
        public SimpleListData OpenListData() => DiskOperations.OpenXml<SimpleListData>(ListDataPath);
        public SimpleListData OpenOrCreateSimpleListData(Blob root)
        {
            var weeksListData = OpenListData();
            if (weeksListData == null)
                weeksListData = SimpleListData.Create(root);
            weeksListData.Restore();
            weeksListData.RootBlob = root;
            return weeksListData;
        }

        public void SaveListData(SimpleListData simpleViewList)
        {
            DiskOperations.SaveXml(ListDataPath, simpleViewList);
        }

        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
        }
    }
}
