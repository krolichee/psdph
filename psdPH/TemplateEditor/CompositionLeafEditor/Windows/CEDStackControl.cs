using Photoshop;
using psdPH.Logic;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Rule = psdPH.Logic.Rule;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class CEDStackControl<T>:Button
    {
        protected Document _doc;
        protected Composition _root;
        abstract public ICommand DeleteCommand();
        abstract public ICommand EditCommand();
        protected void setMenu(FrameworkElement control, T @object)
        {
            control.ContextMenu = new ContextMenu();
            control.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Удалить",
                Command = this.DeleteCommand(),
                CommandParameter = @object
            }
                );
        }
    }
    class StructureStackControl : CEDStackControl<Composition>
    {
        public override ICommand DeleteCommand() =>
            new StructureCommand(_doc, _root).DeleteCommand;
        public override ICommand EditCommand()=>
            new StructureCommand(_doc, _root).EditCommand;
        public StructureStackControl(Composition composition,Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            var grid = new Grid();
            grid.Children.Add(new Label()
            {
                Content = composition.UIName,
                Foreground = SystemColors.ActiveBorderBrush,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            grid.Children.Add(new Label()
            {
                Content = composition.ObjName,
                Foreground = SystemColors.ActiveCaptionTextBrush,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            Height = 28;
            Content = grid;
            Command = editCommand;
            setMenu(this, composition);
        }
    }
    abstract public class CEDStackHandler
    {
        protected Document _doc;
        protected Composition _root;
        public StackPanel Stack;
        protected abstract void addControl(object item);
        protected abstract object[] getElements();
        protected void Refresh() {
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
    }
    public class StructureStackHandler : CEDStackHandler
    {
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new StructureCommand(_doc,_root).CreateCommand,
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
            var button = new StructureStackControl((Composition)item, _doc, _root);
            button.Width = Stack.RenderSize.Width;
            Stack.Children.Add(button);
        }
        protected override object[] getElements()=>
            _root.getChildren();

        public StructureStackHandler(Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
            _root.ChildrenUpdatedEvent += Refresh;
        }
    }
    public class RuleStackControl: CEDStackControl<Rule>
    {
        public override ICommand DeleteCommand()=>
            new RuleCommand(_doc, _root).DeleteCommand;

        public override ICommand EditCommand() => new RuleCommand(_doc, _root).EditCommand;
        public RuleStackControl(Rule rule, Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            var rtb = new RuleTextBlock((Rule)rule);
            Height = 28;
            Content = rtb;
            Command = editCommand;
            setMenu(this, rule);
        }

        
    }
    public class RuleStackHandler : CEDStackHandler
    {
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new RuleCommand(_doc, _root).CreateCommand,
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
            var button = new RuleStackControl((Rule)rule,_doc,_root);
            button.Width = Stack.RenderSize.Width;
            Stack.Children.Add(button);
        }
        protected override object[] getElements() =>
            _root.RuleSet.Rules.Cast<Rule>().ToArray();
        public RuleStackHandler(Document doc, Composition root)
        {
            _root.RuleSet.Updated += Refresh;
        }
    }
}
