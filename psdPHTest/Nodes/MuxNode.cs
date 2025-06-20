using System;
using psdPH.Utils.Setups;
using System.Collections.Generic;

namespace psdPH.Nodes
{
    public class MuxNode : Node
    {
        public event Action OutputLinked;
        public bool Toggle;

        public object OnObj;
        public object OffObj;
        public object Output;

        public Setup OnSetup => Setup.TypeConstrained<object>(new SetupConfig(this, nameof(OnObj)));
        public Setup OffSetup=> Setup.TypeConstrained<object>(new SetupConfig(this,nameof( OffObj)));
        public Setup ToggleSetup => new CheckSetup(new SetupConfig(this, nameof(Toggle), "если"));
        public Setup OutputSetup => Setup.Sealed(new SetupConfig(this, nameof(Output)));

        public override List<Setup> Inputs => new List<Setup>() { OnSetup, OffSetup };

        public override List<Setup> Outputs => new List<Setup>() { OutputSetup };

        public MuxNode()
        {
        }

        protected override void _apply()
        {
            Output = Toggle ? OnObj : OffObj;
        }
        protected override bool checkLink(Setup thisSetup, Setup outSetup)
        {
           return base.checkLink(thisSetup, outSetup);
        }
    }
}
