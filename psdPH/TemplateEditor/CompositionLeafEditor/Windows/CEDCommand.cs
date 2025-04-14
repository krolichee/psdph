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
using static psdPH.TemplateEditor.RuleDict;

namespace psdPH
{
    public class CEDCommand
    {
        protected Composition _root;
        protected Document _doc;
        public ICommand Command { get; set; }

        public static CEDCommand CreateCommand(Document doc, Composition root)
        {
            var result = new CEDCommand(doc, root);
            result.Command = new RelayCommand(result.CreateExecuteCommand, result.IsEditableCommand);
            return result;
        }
        public static CEDCommand EditCommand(Document doc, Composition root)
        {
            var result = new CEDCommand(doc, root);
            result.Command = new RelayCommand(result.EditExecuteCommand, result.IsEditableCommand);
            return result;

        }
        public static CEDCommand DeleteCommand(Composition root)
        {
            var result = new CEDCommand(null, root);
            result.Command = new RelayCommand(result.DeleteExecuteCommand, result.IsEditableCommand);
            return result;
        }
        protected virtual bool IsEditableCommand(object parameter) { return true; }
        protected virtual void CreateExecuteCommand(object parameter) { }
        protected virtual void EditExecuteCommand(object parameter) { }
        protected virtual void DeleteExecuteCommand(object parameter) { }
        protected CEDCommand(Document doc, Composition root)
        {
            _root = root;
            _doc = doc;
        }
    }
    public class StructureCommand : CEDCommand
    {
        protected StructureCommand(Document doc, Composition root) : base(doc, root) { }
        protected override bool IsEditableCommand(object parameter) => EditorDict.ContainsKey(parameter.GetType());
        protected override void CreateExecuteCommand(object parameter)
        {
            Composition root = parameter as Composition;
            CreateComposition creator_func;
            if (!StructureDicts.CreatorDict.TryGetValue(root.GetType(), out creator_func))
                throw new ArgumentException();
            ICompositionShapitor creator = creator_func(_doc, root);
            if (creator.ShowDialog() != true)
                return;
            Composition result = creator.GetResultComposition();
            _root.addChild(result);
        }
        protected override void EditExecuteCommand(object parameter)
        {
            EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.removeChild(parameter as Composition);
        }
    }
    public class RuleCommand : CEDCommand
    {
        protected RuleCommand(Document doc, Composition root) : base(doc, root) { }
        protected override bool IsEditableCommand(object parameter) => StructureDicts.EditorDict.ContainsKey(parameter.GetType());
        protected override void CreateExecuteCommand(object parameter)
        {
            Composition root = parameter as Composition;
            CreateRule creator_func;
            if (!StructureDicts.CreatorDict.TryGetValue(root.GetType(), out creator_func))
                throw new ArgumentException();
            IRuleEditor creator = creator_func(_doc, root);
            if (creator.ShowDialog() != true)
                return;
            Composition result = creator.GetResultComposition();
            _root.addChild(result);
        }
        protected override void EditExecuteCommand(object parameter)
        {
            EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.RuleSet.Rules.Remove(parameter as Rule);
        }
    }
}

