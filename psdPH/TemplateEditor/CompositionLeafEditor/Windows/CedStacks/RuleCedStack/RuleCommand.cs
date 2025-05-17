using psdPH.Logic;
using psdPH.RuleEditor;
using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Media;
using static psdPH.TemplateEditor.RuleDicts;

namespace psdPH
{
    public class RuleCommand : TemplateCEDCommand
    {
        public RuleCommand(PsdPhContext context) : base(context) { }
        protected override bool IsEditableCommand(object parameter) => true;
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
            if (StructureDicts.EditorDict.ContainsKey(parameter.GetType()))
                RuleDicts.EditorDict[parameter.GetType()](_doc, parameter as Rule).ShowDialog();
            else
                SystemSounds.Exclamation.Play();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            _root.RuleSet.Rules.Remove(parameter as Rule);
        }
    }
}

