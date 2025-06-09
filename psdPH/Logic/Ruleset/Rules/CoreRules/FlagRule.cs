using Photoshop;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public class FlagRule : ConditionRule, CoreRule
    {
        public override string ToString() => "значение флага";
        
        public string FlagName;
        public bool Value;
        public FlagRule() : base(null) { }
        public FlagRule(Composition composition) : base(composition) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                FlagLeaf[] flagLeaves = Composition.getChildren<FlagLeaf>();
                var flagNames = flagLeaves.Select(f => f.Name).ToArray();
                var flagConfig = new SetupConfig(this, nameof(this.FlagName), "");
                var valueConfig = new SetupConfig(this, nameof(this.Value), "установить в");
                result.Add(Setup.Choose(flagConfig, flagNames));
                result.Add(Setup.Check(valueConfig));
                result.Add(Setup.JustDescrition("и наоборот"));
                return result.ToArray();
            }
        }

        public void CoreApply()
        {
            var flagLeaf = Composition.getChildren<FlagLeaf>().First(f => f.Name == FlagName);
            flagLeaf.Toggle = Condition.IsValid();
        }

        protected override void _apply(Document doc) =>
            CoreApply();
        protected override void _else(Document doc) =>
            CoreApply();
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&FlagName!=null;
        }
    }

}
