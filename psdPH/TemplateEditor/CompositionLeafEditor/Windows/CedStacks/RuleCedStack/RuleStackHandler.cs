using Photoshop;
using psdPH.Logic;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public abstract class RuleStackHandler : TemplateStackHandler
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
            return new RuleStackControl((Rule)rule, RuleSet, _doc);
        }
        protected override object[] getElements() =>
            RuleSet.Rules.ToArray();
        public RuleStackHandler(PsdPhContext context) : base(context)
        {
            RuleSet.Updated += Refresh;
        }
        public RuleStackHandler(RuleSet ruleSet, Document doc) : base(new PsdPhContext(doc, ruleSet.Composition))
        {
            RuleSet = ruleSet;
        }
    }
}
