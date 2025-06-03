using psdPH.Logic;

namespace psdPH.RuleEditor
{
    public interface IBatchRuleEditor
    {
        bool? ShowDialog();
        Rule[] GetResultBatch();
    }
}
