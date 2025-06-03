using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public class DummyCondition : Condition
    {
        public override string ToString() => "(безусловно)";
        public DummyCondition(Composition composition) : base(composition) { }
        public DummyCondition() : base(null) { }
        [XmlIgnore]
        public override Parameter[] Setups => new Parameter[0];


        public override bool IsValid() => true;
    }
    
}
