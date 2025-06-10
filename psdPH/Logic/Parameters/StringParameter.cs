using System.Xml.Serialization;

namespace psdPH.Logic.Parameters
{
    public class StringParameter : Parameter
    {
        [XmlIgnore]
        public string Text { get => Value as string; set => Value=value; }
        public override Setup[] Setups
        {
            get => new Setup[] { Setup.StringInput(getValueSetupConfig()) };
        }
        public StringParameter():base(null) { }
        public StringParameter(string name):base(name) { }
    }
}
