using Photoshop;
using psdPH.RuleEditor;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rule = psdPH.Logic.Rule;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class CEDStackControl<T>:Button
    {
        protected PsdPhContext Context => new PsdPhContext(_doc, _root);
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
            new StructureCommand(Context).DeleteCommand;
        public override ICommand EditCommand()=>
            new StructureCommand(Context).EditCommand;
        public StructureStackControl(Composition composition,PsdPhContext context)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            _doc = context.doc;
            _root = context.root;
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
            CommandParameter = composition;
            Command = editCommand;
            setMenu(this, composition);
        }
    }
    
    public class RuleStackControl : CEDStackControl<Rule>
    {
        public override ICommand DeleteCommand() =>
            new RuleCommand(Context).DeleteCommand;

        public override ICommand EditCommand() =>
            new RuleCommand(Context).EditCommand;
        public RuleStackControl(Rule rule, Document doc, Composition root)
        {
            _doc = doc;
            _root = root;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            var rtb = new RuleTextBlock((Rule)rule);
            //Height = 28;
            Content = rtb;
            CommandParameter = rule;
            Command = editCommand;
            setMenu(this, rule);
        }


    }
}
