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
using psdPH.Views.WeekView.Logic;

namespace psdPH.Views.WeekView
{
    public partial class WeekViewWindow : Window
    {
        private bool _doSave = true;
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        public WeekListData WeekListData;
        public WeekViewWindow(WeekListData weekListData)
        {
            InitializeComponent();
            //var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            //doc = PhotoshopWrapper.OpenDocument(psApp, PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));

            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekStackHandler(weekListData)));
            dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekDayRulesetStackHandler(weekListData.WeekRulesets.DayRules)));
            //dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekRulesetStackHandler(weekListData.WeekRules, doc)));
            
            WeekListData = weekListData;
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Delete();
            _doSave = false;
            Close();
        }
        
        void save()
        {
            if (_doSave)
                WeekView.Instance().SaveWeekListData(WeekListData);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            save();
        }
        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Clear();
            _doSave = false;
            Close();
        }
        
    }
}
