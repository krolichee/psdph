using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public class DummyCondition : Condition
    {
        public bool Value = true;
        public override string ToString() => "(безусловно)";
        public DummyCondition(bool value) : base(null) { Value = value; }
        public DummyCondition() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups => new Setup[0];


        public override bool IsValid() => Value;
    }
    
}
