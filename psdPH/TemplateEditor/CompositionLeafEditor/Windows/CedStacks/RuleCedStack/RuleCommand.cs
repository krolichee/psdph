using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.RuleEditor;
using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Media;
using static psdPH.TemplateEditor.StructureRulesetDefinition;

namespace psdPH
{
    public abstract class RuleCommand : CEDCommand
    {
        protected RuleSet RuleSet;
        public RulesetDefinition RulesetDefinition;
        public RuleCommand(RuleSet ruleSet) {
            RuleSet = ruleSet;
        }
        protected override bool IsEditableCommand(object parameter) => true;
        protected override void CreateExecuteCommand(object parameter)
        {
            var rc_w = new RuleEditorWindow(RulesetDefinition);
            rc_w.ShowDialog();
            RuleSet.AddRules(rc_w.GetResultBatch());
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            RuleSet.Rules.Remove(parameter as Rule);
        }
    }
    public class StructureRuleCommand : RuleCommand
    {
        public StructureRuleCommand(RuleSet ruleSet) : base(ruleSet) {
            RulesetDefinition = new StructureRulesetDefinition(ruleSet.Composition);
        }
    }
}

