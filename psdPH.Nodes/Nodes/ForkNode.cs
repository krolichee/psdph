using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.Nodes
{
    public class ForkNode : Node
    {
        public ForkNode():base() { }
        public bool Toggle;
        public bool NotToggle => !Toggle;
        Setup OnChain=> Setup.TypeConstrained<bool>(new ReflectionConfig(this, nameof(Toggle), "если да"));
        Setup OffChain => Setup.TypeConstrained<bool>(new ReflectionConfig(this, nameof(NotToggle), "если нет"));
        Setup ToggleSetup=>Setup.TypeConstrained<bool>(new ReflectionConfig(this,nameof(Toggle),"если"));
        public override Setup[] Chains => new []{OnChain,OffChain};
        
        public override List<Setup> Inputs => new List<Setup>() { ToggleSetup };

        public override List<Setup> Outputs => new List<Setup>();

        protected override void _apply() { }
    }
}
