using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.IO;
using System.Windows;

namespace psdPH.Views.WeekView
{
    public class WeekView:ViewManager
    {
        protected WeekView(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
        }
        public WeekView() { }
        protected override string ViewName => "WeekView";
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
        public WeekConfig OpenWeekConfig() => DiskOperations.OpenXml<WeekConfig>(ConfigPath);
        public WeekConfig OpenOrCreateWeekConfig(Blob root)
        {
            var weekConfig = OpenWeekConfig();
            if (weekConfig == null)
                weekConfig = CreateWeekConfig(root);
            return weekConfig;
        }
        public WeekListData OpenWeekListData() => DiskOperations.OpenXml<WeekListData>(WeekListDataPath);
        public WeekListData OpenOrCreateWeekListData(Blob root)
        {
            var weeksListData = OpenWeekListData();
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
