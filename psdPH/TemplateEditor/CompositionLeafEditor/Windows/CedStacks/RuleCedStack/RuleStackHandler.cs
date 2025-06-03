using Photoshop;
using psdPH.Logic;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public abstract class RuleStackHandler : CEDStackHandler
    {
        protected abstract RuleCommand RuleCommand { get; }
        protected RuleSet RuleSet;
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new StructureRuleCommand(RuleSet).CreateCommand,
                CommandParameter = type
            };
        }
        //override protected void InitializeAddDropDownMenu(Button button)
        //{
        //    ContextMenu contextMenu = new ContextMenu();
        //    List<MenuItem> items = new List<MenuItem>();
        //    foreach (var ruleType in RuleDicts.CreatorDict.Keys)
        //        items.Add(CreateAddMenuItem(ruleType));
        //    contextMenu.ItemsSource = items;
        //    button.ContextMenu = contextMenu;
        //}
        protected override void AddButtonAction()
        {
            RuleCommand.CreateCommand.Execute(null);
        }
        override protected UIElement createControl(object rule)
        {
            return new RuleStackControl((Rule)rule, RuleCommand);
        }
        protected override object[] getElements() =>
            RuleSet.Rules.ToArray();
        public RuleStackHandler(RuleSet ruleSet)
        {
            RuleSet = ruleSet;
            RuleSet.Updated += Refresh;
        }
    }
    public class StructureRuleStackHandler : RuleStackHandler
    {
        public StructureRuleStackHandler(RuleSet ruleSet) : base(ruleSet) { }
        protected override RuleCommand RuleCommand => new StructureRuleCommand(RuleSet);
    }
}
