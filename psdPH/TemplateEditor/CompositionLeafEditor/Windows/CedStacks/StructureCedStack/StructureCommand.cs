using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.Media;
using static psdPH.TemplateEditor.StructureDicts;

namespace psdPH
{
    public class StructureCommand : TemplateCEDCommand
    {
        public StructureCommand(PsdPhContext context) : base(context) { }
        protected override bool IsEditableCommand(object parameter) => true; 
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
            if (StructureDicts.EditorDict.ContainsKey(parameter.GetType()))
                UIElementExtension.ShowHidding(StructureDicts.EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog);
            else
                SystemSounds.Exclamation.Play();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.removeChild(parameter as Composition);
        }
    }
}

