using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using psdPH.Logic;
using System.Linq;
using psdPH.RuleEditor;
using static psdPH.TemplateEditor.RuleDicts;
using System.Media;
using static psdPH.WeekConfig;
using psdPH.Views.WeekView.Logic;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public static class WeekRules
    {
        public static Rule[] Rules(Composition root) =>
            RuleDicts.Rules(root);
        public static Condition[] WeekConditions(Composition root) => new Condition[]
        {
            new WeekCondition(root)
        };

        public static Condition[] DayConditions(Composition root) => new Condition[]
        {
            new EveryNDayCondition(root),
            new DayOfWeekCondition(root)
        };
    };

    public class WeekRulesetStackHandler : RuleStackHandler
    {
        public WeekRulesetStackHandler(RuleSet ruleSet, Document doc) : base(ruleSet, doc) { }
        protected override RuleCommand RuleCommand => new WeekRuleCommand(RuleSet);
    }
    public class WeekDayRulesetStackHandler : RuleStackHandler
    {
        public WeekDayRulesetStackHandler(RuleSet ruleSet, Document doc) : base(ruleSet, doc) { }
        protected override RuleCommand RuleCommand => new WeekDayRuleCommand(RuleSet);
    }

    public class WeekRuleCommand : RuleCommand
    {
        public WeekRuleCommand(RuleSet ruleSet) : base(ruleSet) { }

        public override Condition[] Conditions => WeekRules.WeekConditions(RuleSet.Composition);

        public override Rule[] Rules => WeekRules.Rules(RuleSet.Composition);
    }
    public class WeekDayRuleCommand : RuleCommand
    {
        public WeekDayRuleCommand(RuleSet ruleSet) : base(ruleSet) { }

        public override Condition[] Conditions => WeekRules.DayConditions(RuleSet.Composition);

        public override Rule[] Rules => WeekRules.Rules(RuleSet.Composition);
    }

    public partial class WeekViewWindow : Window
    {
        private bool _deleted;
        public bool Deleted { private set { _deleted = value; } get => _deleted; }
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        public WeekListData WeekListData;
        Document doc;
        public WeekViewWindow(WeekListData weekListData)
        {
            var root = weekListData.RootBlob;
            var weekConfig = weekListData.WeekConfig;
            InitializeComponent();

            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, Directories.ProjectPsd(PsdPhProject.Instance().ProjectName));

            cedStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekStackHandler(weekListData)));
            dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekDayRulesetStackHandler(weekListData.DayRules, doc)));
            dayRuleStackGrid.Children.Add(CEDStackUI.CreateCEDStack(new WeekRulesetStackHandler(weekListData.WeekRules, doc)));

            if (weekListData == null)
            {
                weekListData = WeekListData.Create(weekConfig, root);
            }
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            WeekListData = weekListData;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var weekView = WeekView.Instance();
            weekView.Delete();
            Deleted = true;
            Close();
        }
    }
}
