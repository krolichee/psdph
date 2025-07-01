using psdPH.Context;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.Media;
using System.Windows;
using static psdPH.TemplateEditor.StructureDicts;

namespace psdPH.TemplateEditor
{
    public class StructureCommand : TemplateCEDCommand
    {
        public StructureCommand(PsdPhContext context) : base(context) { }
        protected override bool IsEditableCommand(object parameter) => true; 
        protected override void CreateExecuteCommand(object parameter)
        {
            Type type = parameter as Type;
            CreateComposition creator_func;
            IBatchCompositionCreator creator;
            try
            {
                if (!CreatorDict.TryGetValue(type, out creator_func))
                    throw new ArgumentException();
                creator = creator_func(_doc, _root);
            }
            catch(ArgumentException) {
                MessageBox.Show("В данный момент этот элемент нельзя создать");
                return;
            }
            
            if (creator.ShowDialog() != true)
                return;
            _root.AddChildren(creator.GetResultBatch());
        }
        protected override void EditExecuteCommand(object parameter)
        {
            if (EditorDict.ContainsKey(parameter.GetType()))
                EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog();
            else
                SystemSounds.Exclamation.Play();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.RemoveChild(parameter as Composition);
        }
    }
}

