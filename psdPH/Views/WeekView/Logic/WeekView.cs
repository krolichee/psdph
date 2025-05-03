using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.IO;
using System.Windows;

namespace psdPH.Views.WeekView
{
    public class WeekView
    {
        private static WeekView _instance;
        private readonly string _projectName;
        public static WeekView Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static WeekView MakeInstance(string projectName)
        {
            return _instance = new WeekView(projectName);
        }
        

        protected WeekView(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
        }

        private string ViewDirectory => Path.Combine(Directories.ViewsDirectory(_projectName), "WeekView");
        private string ConfigPath => Path.Combine(ViewDirectory, "config.xml");
        private string WeekListDataPath => Path.Combine(ViewDirectory, "data.xml");

        public static WeekConfig CreateWeekConfig(Blob root)
        {
            WeekConfigEditor wce_w = new WeekConfigEditor(root);
            if (!wce_w.ShowDialog())
                return null;
            return wce_w.GetResultConfig();
        }

        public WeekConfig OpenOrCreateWeekConfig(Blob root)
        {
            var weekConfig = DiskOperations.OpenXml<WeekConfig>(ConfigPath);
            if (weekConfig == null)
                weekConfig = CreateWeekConfig(root);
            return weekConfig;
        }

        public WeekListData OpenOrCreateWeekListData(Blob root)
        {
            var weeksListData = DiskOperations.OpenXml<WeekListData>(WeekListDataPath);
            var weekConfig = OpenOrCreateWeekConfig(root);

            if (weekConfig == null)
                return null;

            if (weeksListData == null)
                weeksListData = WeekListData.Create(weekConfig, root);
            else
                weeksListData.WeekConfig = weekConfig;

            weeksListData.Restore();
            weeksListData.RootBlob = root;
            return weeksListData;
        }

        public void SaveWeekListData(WeekListData weekListData)
        {
            var weekConfig = weekListData.WeekConfig;
            DiskOperations.SaveXml(ConfigPath, weekConfig);
            DiskOperations.SaveXml(WeekListDataPath, weekListData);
        }

        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
            
        }
    }
}
