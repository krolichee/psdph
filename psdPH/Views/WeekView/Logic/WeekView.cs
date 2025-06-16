using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.WeekView.Logic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;

namespace psdPH.Views.WeekView
{
    public class WeekView
    {
        WeekListData WeekListData;
        private static WeekView _instance;
        private readonly string _projectName;
        public static WeekView Instance()
        {
            if (_instance == null)
                throw new System.Exception();
            return _instance;
        }
        public static WeekView MakeInstance()
        {
            return _instance = new WeekView(PsdPhProject.Instance().ProjectName);
        }
        protected WeekView(string projectName)
        {
            _projectName = projectName;
            Directory.CreateDirectory(ViewDirectory);
            WeekListData = tryOpenOrCreateData();
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
            if (!wce_w.NewConfigShowDialog())
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
        
        void SaveWeekListData(WeekListData weekListData)
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

        public Window ShowWindow()
        {
            if (WeekListData == null)
                return null;
            var window = new WeekViewWindow(WeekListData);
            window.Show();
            return window;
        }

        internal void Save()
        {
            SaveWeekListData(WeekListData);
            PsdPhProject.Instance().saveBlob(WeekListData.RootBlob);
        }
        WeekListData tryOpenOrCreateData()
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
        WeekListData openOrCreateData()
        {
            var weeksListData = OpenWeekListData();
            var root = PsdPhProject.Instance().openMainBlob();
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

            weeksListData.RootBlob = root;
            weeksListData.Restore();

            return weeksListData;
        }
    }
}
