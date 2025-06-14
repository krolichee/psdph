﻿using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.Media;
using System.Windows;
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
            IBatchCompositionCreator creator;
            try
            {
                if (!StructureDicts.CreatorDict.TryGetValue(type, out creator_func))
                    throw new ArgumentException();
                creator = creator_func(_doc, _root);
            }
            catch(ArgumentException e) {
                MessageBox.Show("В данный момент этот элемент нельзя создать");
                return;
            }
            
            if (creator.ShowDialog() != true)
                return;
            _root.AddChildren(creator.GetResultBatch());
        }
        protected override void EditExecuteCommand(object parameter)
        {
            if (StructureDicts.EditorDict.ContainsKey(parameter.GetType()))
               StructureDicts.EditorDict[parameter.GetType()](_doc, parameter as Composition).ShowDialog();
            else
                SystemSounds.Exclamation.Play();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.RemoveChild(parameter as Composition);
        }
    }
}

