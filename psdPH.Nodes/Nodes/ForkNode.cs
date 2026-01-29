using psdPH.Logic.Compositions;
using psdPH.Logic;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Nodes.Nodes
{
    public class ForkNode : Node
    {
        public ForkNode():base() { }
        [XmlIgnore]
        public bool Toggle;
        [XmlIgnore]
        public bool NotToggle => !Toggle;
        Setup OnChain=> Setup.TypeConstrained<bool>(new ReflectionConfig(this, nameof(Toggle), "если да"));
        Setup OffChain => Setup.TypeConstrained<bool>(new ReflectionConfig(this, nameof(NotToggle), "если нет"));
        Setup ToggleSetup=>Setup.TypeConstrained<bool>(new ReflectionConfig(this,nameof(Toggle),"если"));
        [XmlIgnore]
        public override Setup[] Chains => new []{OnChain,OffChain};
        [XmlIgnore]
        public override List<Setup> Inputs => new List<Setup>() { ToggleSetup };
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>();

        protected override void _apply() { }
        protected override DtoConverter DtoConverter =>new NullDtoConverter() ;
    }
}
