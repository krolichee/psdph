using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace psdPH.Views.WeekView
{
    public class WeekView
    {
        static string ViewDirectory(string projectName)
        {
            return Path.Combine(Directories.ViewsDirectory(projectName), "WeekView");
        }
        static string ConfigPath(string projectName)
        {
            return Path.Combine(ViewDirectory( projectName), "config.xml");
        } 

        static string WeekListDataPath(string projectName)
        {
            return Path.Combine(ViewDirectory(projectName), "data.xml");
        }
        public static WeekConfig createWeekConfig(Blob root)
        {
            WeekConfigEditor wce_w = new WeekConfigEditor(root);
            if (!wce_w.ShowDialog())
                return null;
            return wce_w.GetResultConfig();
        }
        public static WeekConfig openOrCreateWeekConfig(string projectName, Blob root)
        {
            string configPath = ConfigPath(projectName);
            var weekConfig = DiskOperations.openXml<WeekConfig>(configPath);
            if (weekConfig == null)
                weekConfig = createWeekConfig(root);
            return weekConfig;
        }
        public static WeekListData openOrCreateWeekListData(string projectName, Blob root)
        {
            string dataPath = WeekListDataPath(projectName);
            var weeksListData = DiskOperations.openXml<WeekListData>(dataPath);
            WeekConfig weekConfig;
            weekConfig = openOrCreateWeekConfig(projectName, root);
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
        public static void saveWeekListData(string projectName,WeekListData weekListData)
        {
            var weekConfig = weekListData.WeekConfig;
            DiskOperations.saveXml(ConfigPath(projectName), weekConfig);
            DiskOperations.saveXml(WeekListDataPath(projectName), weekListData);
        }
    }
}
