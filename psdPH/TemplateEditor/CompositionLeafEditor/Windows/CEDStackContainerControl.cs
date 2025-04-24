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
    abstract public class CEDStackElement<T>:Control
    {
        abstract public ICommand DeleteCommand(T @object);
        protected void setMenuItem(FrameworkElement control, T @object)
        {
            control.ContextMenu = new ContextMenu();
            control.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Удалить",
                Command = this.DeleteCommand(@object),
                CommandParameter = @object
            }
                );
        }
    }
    abstract public class CEDStackContainerControl<T>
    {
        protected Document _doc;
        protected Composition _root;
        public StackPanel Stack;

        public CEDStackContainerControl(Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
        }
        
        protected abstract void Refresh();
    }
    public class RuleStackElement: CEDStackElement<Composition>
    {
        public override ICommand DeleteCommand(Composition @object) =>
            StructureCommand.DeleteCommand(_root).Command;
        public static Control Create(Rule rule, ICommand editCommand, ICommand deleteCommand)
        {
            Button result = new Button() { Content = new RuleTextBlock(rule as ConditionRule) };
            result.ContextMenu = new ContextMenu();
            result.Command = editCommand;
            result.ContextMenu.Items.Add(new MenuItem()
                {
                    Command = deleteCommand,
                    CommandParameter = rule,
                    Header = "Удалить"
                });;
            return result;
        }
    }
    public class StructureStackContainerControl : CEDStackContainerControl<Composition>
    {

        protected override void Refresh()
        {
            Stack.Children.Clear();
            foreach (Composition child in _root.getChildren())
            { 
                var button = new CompositionStackElement(child,
                    StructureCommand.EditCommand(_doc, _root).Command)
                    
                button.Width = Stack.RenderSize.Width;
                Stack.Children.Add(button);
            }
        }

        

        public StructureStackContainerControl(Document doc, Composition root) : base(doc, root)
        {
            _root.ChildrenUpdatedEvent += Refresh;
        }
    }
    public class RuleStackContainerControl : CEDStackContainerControl<Rule>
    {
        public RuleStackContainerControl(Document doc, Composition root) : base(doc, root)
        {
            _root.RuleSet.Updated += Refresh;
        }
        public override ICommand DeleteCommand(Rule @object)
            => RuleCommand.DeleteCommand(@object.Composition).Command;

        protected void addControl(ConditionRule rule)
        {
            var rtb = new RuleTextBlock(rule);
            setMenuItem(rtb,rule);
            
            Stack.Children.Add(rtb);
        }

        protected override void Refresh()
        {
            Stack.Children.Clear();
            ConditionRule[] rules = _root.RuleSet.Rules.Cast<ConditionRule>().ToArray();
            foreach (var rule in rules)
                addControl(rule);
        }
    }
}
