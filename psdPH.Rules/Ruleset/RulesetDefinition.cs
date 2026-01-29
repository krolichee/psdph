using psdPH.Logic.Rules;
using psdPH.Rules;

namespace psdPH.Logic.Ruleset
{
    public class RulesetDefinition
    {
        public CompositionRule[] Rules;
        public CompositionCondition[] Conditions;
        public RulesetDefinition(CompositionRule[] rules, CompositionCondition[] conditions)
        {
            Rules = rules;
            Conditions = conditions;
        }
    }
}
