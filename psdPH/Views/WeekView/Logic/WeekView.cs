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

        public string ViewDirectory => Path.Combine(PsdPhDirectories.ViewsDirectory(_projectName), "WeekView");
        public string ConfigPath => Path.Combine(ViewDirectory, "config.xml");
        public string WeekListDataPath => Path.Combine(ViewDirectory, "data.xml");
        public string WeekRulesetsPath => Path.Combine(ViewDirectory, "rules.xml");
        public string OutputsDirectory => Path.Combine(ViewDirectory, "output");
        public string OutputDirectory(string outputName) => Path.Combine(OutputsDirectory, outputName);
        public void CreateOutputsDirectory() => Directory.CreateDirectory(OutputsDirectory);
        public void CreateOutputDirectory(string outputName) => Directory.CreateDirectory(OutputDirectory(outputName));

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
                weeksListData.MainBlob = root;
                weeksListData.WeekRulesets = weekRules;
            }

            weeksListData.MainBlob = root;
            weeksListData.Restore();
            
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

        public static Window ShowWindow()
        {
            var project = PsdPhProject.Instance();
            var weekView = WeekView.MakeInstance(project.ProjectName);
            Blob blob = project.openOrCreateMainBlob();
            WeekListData weekListData;
            try { 
                weekListData = weekView.OpenOrCreateWeekListData(blob); 
            }
            catch
            {
                var result = MessageBox.Show("Во время открытия данных вида произошла ошибка. Удалить вид?", "Ошибка", MessageBoxButton.YesNo,MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                    WeekView.Instance().Delete();
                return null;
            }

            if (weekListData == null)
                return null;
            var window = new WeekViewWindow(weekListData);
            window.Show();
            return window;
        }
    }
}
