using psdPH.Logic;
using psdPH.Logic.Ruleset.Rules;

namespace psdPH.RuleEditor
{
    public interface IBatchRuleEditor
    {
        bool? ShowDialog();
        Rule[] GetResultBatch();
    }
}
