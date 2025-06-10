namespace psdPH.Logic.Parameters
{
    public class FlagParameter : Parameter
    {
        public bool? Toggle { get =>(bool?) Value; set => Value = value; }
        public override Setup[] Setups
        {
            get => new Setup[] { Setup.Check(getValueSetupConfig()) };
        }
        public FlagParameter() : base(null) { }
        public FlagParameter(string name) : base(name) { }
    }
}
