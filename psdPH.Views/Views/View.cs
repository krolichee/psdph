using psdPH.Project;
using psdPH.Utils;
using psdPH.Views.SimpleView.Logic;
using System.IO;
using System.Windows;

namespace psdPH.Views
{
    public abstract class View<T> where T:ViewListData
    {
        public T ListData;
        protected static View<T> _instance;
        protected readonly string _projectName;
        public abstract string ViewName { get; }
        public string ViewDirectory => Path.Combine(PsdPhDirectories.ViewsDirectory(_projectName), ViewName);
        public string OutputsDirectory => Path.Combine(ViewDirectory, "output");
        public string OutputDirectory(string outputName) => Path.Combine(OutputsDirectory, outputName);
        public string ListDataPath => Path.Combine(ViewDirectory, "data.xml");
        public void CreateOutputsDirectory() => Directory.CreateDirectory(OutputsDirectory);
        public void CreateOutputDirectory(string outputName) => Directory.CreateDirectory(OutputDirectory(outputName));
        public SimpleListData OpenData() => DiskOperations.LoadXml<SimpleListData>(ListDataPath);
        public static View<T> Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        protected View()
        {
            _projectName = PsdPhProject.Instance().ProjectName;
            Directory.CreateDirectory(ViewDirectory);
            ListData = tryOpenOrCreateData();
        }
        public void Clear()
        {
            File.Delete(ListDataPath);
        }
        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
        }
        T tryOpenOrCreateData()
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
                return default(T);
            }
        }
        public abstract Window ShowWindow();
        protected abstract T openOrCreateData();
        protected virtual void SaveListData(T listData)
        {

        }
        public void Save()
        {
            SaveListData(ListData);
            PsdPhProject.Instance().saveBlob(ListData.RootBlob);
        }
    }
}
