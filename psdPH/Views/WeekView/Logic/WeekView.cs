using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.WeekView.Logic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;

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

        private string ViewDirectory => Path.Combine(PsdPhDirectories.ViewsDirectory(_projectName), "WeekView");
        private string ConfigPath => Path.Combine(ViewDirectory, "config.xml");
        private string WeekListDataPath => Path.Combine(ViewDirectory, "data.xml");
        private string WeekRulesetsPath => Path.Combine(ViewDirectory, "rules.xml");

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
        public WeekRulesets OpenWeekRulesets() => DiskOperations.OpenXml<WeekRulesets>(WeekRulesetsPath);
        public WeekListData OpenOrCreateWeekListData(Blob root)
        {
            var weeksListData = OpenWeekListData();
            var weekConfig = OpenOrCreateWeekConfig(root);
            var weekRules = OpenWeekRulesets();

            if (weekRules == null)
                weekRules = new WeekRulesets();

            if (weekConfig == null)
                return null;

            if (weeksListData == null)
                weeksListData = WeekListData.Create(weekConfig, weekRules, root);
            else
            {
                weeksListData.WeekConfig = weekConfig;
                weeksListData.RootBlob = root;
                weeksListData.WeekRulesets = weekRules;
            }

            weeksListData.Restore();
            weeksListData.RootBlob = root;
            return weeksListData;
        }
        public void SaveWeekListData(WeekListData weekListData)
        {
            var weekConfig = weekListData.WeekConfig;
            var weekRulesets = weekListData.WeekRulesets;
            DiskOperations.SaveXml(ConfigPath, weekConfig);
            DiskOperations.SaveXml(WeekListDataPath, weekListData);
            DiskOperations.SaveXml(WeekRulesetsPath, weekRulesets);
        }
        public void Clear()
        {
            File.Delete(WeekListDataPath);
        }
        internal void Delete()
        {
            Directory.Delete(ViewDirectory, true);
        }

        public static Window ShowWindowDialog()
        {
            var project = PsdPhProject.Instance();
            var weekView = WeekView.MakeInstance(project.ProjectName);
            Blob blob = project.openOrCreateMainBlob();
            var weekListData = weekView.OpenOrCreateWeekListData(blob);
            if (weekListData == null)
                return null;
            var window = new WeekViewWindow(weekListData);
            window.Show();
            return window;
        }
    }
}
