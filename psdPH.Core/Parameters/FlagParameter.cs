

using psdPH.Parameters;
using psdPH.Setups;
using psdPH.Utils.Setups;

namespace psdPH.Logic.Parameters
{
    public class FlagParameter : Parameter
    {
        public bool? Toggle { get =>(bool?) Value; set => Value = value; }
        public override Setup[] Setups
        {
            get => new Setup[] { new CheckSetup(getValueSetupConfig()) };
        }
        public FlagParameter() : base(null) { }
        public FlagParameter(string name) : base(name) { }
    }
}
