using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.RuleEditor;
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
        protected override void EditExecuteCommand(object parameter)
        {
            var rule = parameter as ConditionRule;
            var rc_w = new RuleEditorWindow(rule, RulesetDefinition);
            rc_w.ShowDialog();
            var index = RuleSet.Rules.IndexOf(rule);

            if (index >= 0)
            {
                RuleSet.Rules.Remove(rule);
                var resultBatch = rc_w.GetResultBatch();
                foreach (var brule in resultBatch)
                {
                    RuleSet.Rules.Insert(index, brule);
                }
            }
        }
    }
}

