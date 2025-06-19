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
    public class WeekView : View<WeekListData>
    {

        public static WeekView MakeWeekView()
        {
            return (_instance = new WeekView()) as WeekView;
        }

        public string ConfigPath => Path.Combine(ViewDirectory, "config.xml");
        public string WeekRulesetsPath => Path.Combine(ViewDirectory, "rules.xml");

        public override string ViewName => "WeekView";



        public static WeekConfig CreateWeekConfig(RootBlob root)
        {
            WeekConfigEditor wce_w = new WeekConfigEditor(root);
            if (!wce_w.NewConfigShowDialog())
                return null;
            return wce_w.GetResultConfig();
        }
        protected WeekConfig OpenWeekConfig() => DiskOperations.OpenXml<WeekConfig>(ConfigPath);
        protected WeekConfig OpenOrCreateWeekConfig(RootBlob root)
        {
            var weekConfig = OpenWeekConfig();
            if (weekConfig == null)
                weekConfig = CreateWeekConfig(root);
            return weekConfig;
        }
        protected WeekListData OpenWeekListData() => DiskOperations.OpenXml<WeekListData>(ListDataPath);
        protected WeekRulesets OpenWeekRulesets() => DiskOperations.OpenXml<WeekRulesets>(WeekRulesetsPath);

        protected override void SaveListData(WeekListData listData)
        {
            var weekListData = listData;
            var weekConfig = weekListData.WeekConfig;
            var weekRulesets = weekListData.WeekRulesets;
            DiskOperations.SaveXml(ConfigPath, weekConfig);
            DiskOperations.SaveXml(ListDataPath, weekListData);
            DiskOperations.SaveXml(WeekRulesetsPath, weekRulesets);
        }


        public override Window ShowWindow()
        {
            if (ListData == null)
                return null;
            var window = new WeekViewWindow(ListData);
            window.Show();
            return window;
        }

        protected override WeekListData openOrCreateData()
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
                weeksListData = new WeekListData();

            weeksListData.WeekConfig = weekConfig;
            weeksListData.RootBlob = root;
            weeksListData.WeekRulesets = weekRules;
            weeksListData.RootBlob = root;
            weeksListData.Restore();

            return weeksListData;
        }
    }
}
