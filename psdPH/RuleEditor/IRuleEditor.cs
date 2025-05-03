using psdPH.Logic;

namespace psdPH.RuleEditor
{
    public interface IRuleEditor
    {
        bool? ShowDialog();
        ConditionRule GetResultRule();
    }
}
