using Photoshop;
using psdPH.Logic;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class CEDStackHandler
    {
        protected Document _doc;
        protected Composition _root;
        public StackPanel Stack;
        public CEDStackHandler(Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
        }
        protected abstract void Refresh();
    }
    public class StructureStackHandler : CEDStackHandler
    {
        protected override void Refresh()
        {
            Stack.Children.Clear();
            foreach (Composition child in _root.getChildren())
            {
                var button = new CompositionStackElement(child,
                    StructureCommand.EditCommand(_doc, _root).Command,
                    StructureCommand.DeleteCommand(_root).Command);
                button.Width = Stack.RenderSize.Width;
                Stack.Children.Add(button);
            }
        }
        public StructureStackHandler(Document doc, Composition root) : base(doc, root)
        {
            _root.ChildrenUpdatedEvent += Refresh;
        }
    }
    public class RuleStackHandler : CEDStackHandler
    {
        public RuleStackHandler(Document doc, Composition root) : base(doc, root)
        {
            _root.RuleSet.Updated += Refresh;
        }

        protected override void Refresh()
        {
            Stack.Children.Clear();
            ConditionRule[] rules = _root.RuleSet.Rules.Cast<ConditionRule>().ToArray();
            foreach (var rule in rules)
            {
                var rtb = new RuleTextBlock(rule);

                rtb.ContextMenu = new ContextMenu();
                rtb.ContextMenu.Items.Add(new MenuItem()
                {
                    Header = "Удалить",
                    Command = RuleCommand.EditCommand(_doc, rule.Composition).Command,
                    CommandParameter = rule,
                    Margin = new Thickness(0, 0, 0, 10)
                }
                    );
                Stack.Children.Add(rtb);
            }
        }
    }
}
