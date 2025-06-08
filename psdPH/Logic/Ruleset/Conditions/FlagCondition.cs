using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public class FlagCondition : Condition
    {
        public override string ToString() => "значение флага";
        public string FlagName;
        public bool Value=true;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                FlagLeaf[] flagLeaves = Composition.getChildren<FlagLeaf>();
                var flagConfig = new SetupConfig(this, nameof(this.FlagLeaf), "");
                var valueConfig = new SetupConfig(this, nameof(this.Value),"установлено в");
                result.Add(Setup.Choose(flagConfig, flagLeaves));
                result.Add(Setup.Check(valueConfig));
                return result.ToArray();
            }
        }

        [XmlIgnore]
        public FlagLeaf FlagLeaf
        {
            get
            {
                return Composition.getChildren<FlagLeaf>().FirstOrDefault(t => t.Name == FlagName);
            }
            set
            {
                FlagName = value.Name;
            }
        }
        public override bool IsValid()
        {
            return FlagLeaf.Toggle== Value;
        }
        public FlagCondition(Composition composition) : base(composition) { }
        public FlagCondition() : base(null) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&FlagName!=null;
        }
    }
    
}
