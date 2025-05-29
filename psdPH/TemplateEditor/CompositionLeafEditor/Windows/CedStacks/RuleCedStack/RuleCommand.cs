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
using static psdPH.TemplateEditor.RuleDicts;

namespace psdPH
{
    public abstract class RuleCommand : CEDCommand
    {
        protected RuleSet RuleSet;
        public abstract Rule[] Rules { get; }
        public abstract Condition[] Conditions { get; }
        public RuleCommand(RuleSet ruleSet) {
            RuleSet = ruleSet;
        }
        protected override bool IsEditableCommand(object parameter) => true;
        protected override void CreateExecuteCommand(object parameter)
        {
            var rc_w = new RuleEditorWindow(Rules, Conditions);
            if (rc_w.ShowDialog() != true)
                return;
            RuleSet.AddRule(rc_w.GetResultRule());
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            RuleSet.Rules.Remove(parameter as Rule);
        }
    }
    public class StructureRuleCommand : RuleCommand
    {
        public StructureRuleCommand(RuleSet ruleSet) : base(ruleSet) { }

        public override Rule[] Rules => RuleDicts.Rules(RuleSet.Composition);

        public override Condition[] Conditions => RuleDicts.Conditions(RuleSet.Composition);
    }
}

