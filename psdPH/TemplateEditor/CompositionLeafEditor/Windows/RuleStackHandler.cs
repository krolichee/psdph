using Photoshop;
using psdPH.Logic;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class RuleStackHandler : TemplateStackHandler
    {
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new RuleCommand(Context).CreateCommand,
                CommandParameter = type
            };
        }
        override protected void InitializeAddDropDownMenu(Button button)
        {
            ContextMenu contextMenu = new ContextMenu();
            List<MenuItem> items = new List<MenuItem>();
            foreach (var ruleType in RuleDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(ruleType));
            contextMenu.ItemsSource = items;
            button.ContextMenu = contextMenu;
        }
        override protected UIElement createControl(object rule)
        {
            return new RuleStackControl((Rule)rule, Context);
            //button.Width = Stack.RenderSize.Width;
        }
        protected override object[] getElements() =>
            _root.RuleSet.Rules.Cast<Rule>().ToArray();
        public RuleStackHandler(PsdPhContext context) : base(context)
        {
            _root.RuleSet.Updated += Refresh;
        }
    }
}
