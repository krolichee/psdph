using psdPH.Logic.Parameters;

namespace psdPH.Logic
{
    public interface ParameterSetRule : CoreRule
    {
        void SetParameterSet(ParameterSet parset);
    }

}
