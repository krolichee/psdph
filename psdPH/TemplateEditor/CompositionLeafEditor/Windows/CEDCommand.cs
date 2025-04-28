using Photoshop;
using psdPH.Logic;
using psdPH.RuleEditor;
using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using static psdPH.TemplateEditor.StructureDicts;
using static psdPH.TemplateEditor.RuleDicts;
using psdPH.Utils;

namespace psdPH
{
    public abstract class CEDCommand
    {
        protected Document _doc;
        protected Composition _root;
        public ICommand CreateCommand => new RelayCommand(CreateExecuteCommand, (_) => true);
        public ICommand EditCommand=> new RelayCommand(EditExecuteCommand, IsEditableCommand);
        public ICommand DeleteCommand=>new RelayCommand(DeleteExecuteCommand, (_) => true);
        protected virtual bool IsEditableCommand(object parameter) { return true; }
        protected virtual void CreateExecuteCommand(object parameter) { }
        protected virtual void EditExecuteCommand(object parameter) { }
        protected virtual void DeleteExecuteCommand(object parameter) { }
        protected CEDCommand(PsdPhContext context):this(context.doc,context.root){}
        protected CEDCommand(Document doc, Composition root)
        {
            _root = root;
            _doc = doc;
        }
    }
    public class StructureCommand : CEDCommand
    {
        public StructureCommand(PsdPhContext context) : base(context) { }
        protected override bool IsEditableCommand(object parameter) => StructureDicts.EditorDict.ContainsKey(parameter.GetType());
        protected override void CreateExecuteCommand(object parameter)
        {
            Type type = parameter as Type;
            CreateComposition creator_func;
            if (!StructureDicts.CreatorDict.TryGetValue(type, out creator_func))
                throw new ArgumentException();
            ICompositionShapitor creator = creator_func(_doc, _root);
            if (creator.ShowDialog() != true)
                return;
            Composition result = creator.GetResultComposition();
            _root.addChild(result);
        }
        protected override void EditExecuteCommand(object parameter)
        {
            StructureDicts.EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.removeChild(parameter as Composition);
        }
    }
    public class RuleCommand : CEDCommand
    {
        public RuleCommand(PsdPhContext context) : base(context) { }
        protected override bool IsEditableCommand(object parameter) => StructureDicts.EditorDict.ContainsKey(parameter.GetType());
        protected override void CreateExecuteCommand(object parameter)
        {
            Type type = parameter as Type;
            CreateRule creator_func;
            if (!RuleDicts.CreatorDict.TryGetValue(type, out creator_func))
                throw new ArgumentException();
            IRuleEditor creator = creator_func(_doc, _root);
            if (creator.ShowDialog() != true)
                return;
            Rule result = creator.GetResultRule();
            _root.RuleSet.Rules.Add(result);
        }
        protected override void EditExecuteCommand(object parameter)
        {
            RuleDicts.EditorDict[parameter.GetType()](_doc, parameter as Rule).ShowDialog();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.RuleSet.Rules.Remove(parameter as Rule);
        }
    }
}

