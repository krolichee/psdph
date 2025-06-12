using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules.CompositionRules;
using psdPH.RuleEditor;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.TemplateEditor
{
    public class StructureRulesetDefinition:RulesetDefinition
    {
        static Condition[] getConditions(Composition root) => new Condition[]
            {
                new FlagCondition(root),
                new NonEmptyTextCondition(root),
                new EmptyTextCondition(root),
                new DummyCondition()
            };
        static Rule[] getRules(Composition root) => new Rule[]
            {
                new TextFontSizeRule(root),
                new TextJustifRule(root),
                new TranslateRule(root),
                new OpacityRule(root),
                new VisibleRule(root),
                new AlignRule(root),
                new FitRule(root),
                new FlagRule(root),
                new TextAssignRule(root),
                new SplitForAreaRule(root)
            };
        public delegate IBatchRuleEditor CreateRule(Document doc, Composition composition);
        public delegate IBatchRuleEditor EditRule(Document doc, Rule rule);

        public StructureRulesetDefinition(Composition root) : base(
            rules: getRules(root),
            conditions: getConditions(root)
            ) { }

        
    }

}
