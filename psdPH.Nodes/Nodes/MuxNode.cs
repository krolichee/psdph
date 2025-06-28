using System;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;

namespace psdPH.Nodes
{
    public class MuxNode : Node
    {
        public override string ToString() => "Выбор";
        public event Action OutputLinked;
        [XmlIgnore]
        public bool Toggle;
        [XmlIgnore]
        public object OnObj;
        [XmlIgnore]
        public object OffObj;
        [XmlIgnore]
        public object Output;
        [XmlIgnore]
        public Setup OnSetup => Setup.TypeConstrained<object>(new ReflectionConfig(this, nameof(OnObj),"если Да"));
        [XmlIgnore]
        public Setup OffSetup=> Setup.TypeConstrained<object>(new ReflectionConfig(this,nameof( OffObj), "если Нет"));
        [XmlIgnore]
        public Setup ToggleSetup => new CheckSetup(new ReflectionConfig(this, nameof(Toggle), "Да/Нет"));
        [XmlIgnore]
        public Setup OutputSetup => Setup.Sealed(new ReflectionConfig(this, nameof(Output),"результат"));
        [XmlIgnore]
        public override List<Setup> Inputs => new List<Setup>() {ToggleSetup,OnSetup, OffSetup };
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>() { OutputSetup };

        public MuxNode() : base()
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
