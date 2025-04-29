using Photoshop;
using psdPH.Logic;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class CEDStackHandler
    {
        protected PsdPhContext Context => new PsdPhContext(_doc, _root);
        protected Document _doc;
        protected Composition _root;
        public StackPanel Stack;
        protected abstract void addControl(object item);
        protected abstract object[] getElements();
        public void Refresh() {
            Stack.Children.Clear();
            object[] elements = getElements();
            foreach (object item in elements)
                addControl(item);
        }
        protected abstract void InitializeAddDropDownMenu(Button button);
        protected abstract MenuItem CreateAddMenuItem(Type type);
        public void Initialize(CEDStackUI cEDStackUI)
        {
            InitializeAddDropDownMenu(cEDStackUI.AddButton);
            Stack = cEDStackUI.StackPanel;
        }
        public CEDStackHandler(Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
        }
    }
    public class StructureStackHandler : CEDStackHandler
    {

        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new StructureCommand(Context).CreateCommand,
                CommandParameter = type
            };
        }
        override protected void InitializeAddDropDownMenu(Button button)
        {
            ContextMenu contextMenu = new ContextMenu();
            List<MenuItem> items = new List<MenuItem>();
            foreach (var comp_type in StructureDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(comp_type));
            contextMenu.ItemsSource = items;
            button.ContextMenu = contextMenu;
        }

        protected override void addControl(object item)
        {
            var context = new PsdPhContext(_doc, _root);
            var button = new StructureStackControl((Composition)item, context);
            //button.Width = Stack.RenderSize.Width;
            Stack.Children.Add(button);
        }
        protected override object[] getElements() =>
            _root.getChildren();

        public StructureStackHandler(Document doc, Composition root) : base(doc, root)
        {
            _root.ChildrenUpdatedEvent += Refresh;
        }
    }

    public class RuleStackHandler : CEDStackHandler
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
            foreach (var comp_type in RuleDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(comp_type));
            contextMenu.ItemsSource = items;
            button.ContextMenu = contextMenu;
        }
        override protected void addControl(object rule)
        {
            var button = new RuleStackControl((Rule)rule, _doc, _root);
            //button.Width = Stack.RenderSize.Width;
            Stack.Children.Add(button);
        }
        protected override object[] getElements() =>
            _root.RuleSet.Rules.Cast<Rule>().ToArray();
        public RuleStackHandler(Document doc, Composition root) : base(doc, root)
        {
            _root.RuleSet.Updated += Refresh;
        }
    }
}
