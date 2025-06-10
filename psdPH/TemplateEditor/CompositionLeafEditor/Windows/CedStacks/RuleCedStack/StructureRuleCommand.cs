using psdPH.Logic;
using psdPH.TemplateEditor;

namespace psdPH
{
    public class StructureRuleCommand : RuleCommand
    {
        public StructureRuleCommand(RuleSet ruleSet) : base(ruleSet) {
            RulesetDefinition = new StructureRulesetDefinition(ruleSet.Composition);
        }
    }
}

