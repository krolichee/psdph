using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using psdPH.RuleEditor;
using static psdPH.TemplateEditor.StructureRulesetDefinition;
using System.Media;
using static psdPH.WeekConfig;

namespace psdPH.Views.WeekView
{
    public partial class WeekViewWindow : Window
    {
        private bool _deleted;
        public bool Deleted { private set { _deleted = value; } get => _deleted; }
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        public WeekListData WeekListData;
        public WeekViewWindow(WeekListData weekListData)
        {

            var root = weekListData.RootBlob;
            var weekConfig = weekListData.WeekConfig;
            InitializeComponent();

            //var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            //doc = PhotoshopWrapper.OpenDocument(psApp, PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));

            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekStackHandler(weekListData)));
            dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekDayRulesetStackHandler(weekListData.DayRules)));
            //dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekRulesetStackHandler(weekListData.WeekRules, doc)));

            if (weekListData == null)
            {
                weekListData = WeekListData.Create(weekConfig, root);
            }
            WeekListData = weekListData;
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Delete();
            Close();
        }
        
        void save()
        {
            WeekView.Instance().SaveWeekListData(WeekListData);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            save();
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Clear();
            Close();
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
