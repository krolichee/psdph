using psdPH.Utils.Setups;
using System.Collections.Generic;
using psdPH.Nodes;
using psdPH.Utils;
using System.Xml.Serialization;
using psdPH.Setups;

namespace psdPHTest.Nodes
{
    public class SplitForRatioNode:Node
    {
        [XmlIgnore]
        public string DryText;
        [XmlIgnore]
        public double Ratio;
        [XmlIgnore]
        public string WetText;
        [XmlIgnore]
        public override List<Setup> Inputs => new List<Setup>() { 
            new StringInputSetup(new ReflectionConfig(this,nameof(DryText))),
            ///TODO RatioSetup
            Setup.TypeConstrained<double>(new ReflectionConfig(this,nameof(Ratio)))
        };
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>() { 
            Setup.Sealed(new ReflectionConfig(this, nameof(WetText))) 
        };

        public SplitForRatioNode() : base()
        {
        }

        protected override void _apply()
        {
            WetText = SplitTextToRatio.Splitter.Split(DryText, Ratio);
        }
    }
}
