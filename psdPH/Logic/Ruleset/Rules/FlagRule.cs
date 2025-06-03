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
        public override Parameter[] Setups
        {
            get
            {
                List<Parameter> result = new List<Parameter>();
                FlagLeaf[] flagLeaves = Composition.getChildren<FlagLeaf>();
                var flagNames = flagLeaves.Select(f => f.Name).ToArray();
                var flagConfig = new ParameterConfig(this, nameof(this.FlagName), "");
                var valueConfig = new ParameterConfig(this, nameof(this.Value), "установить в");
                result.Add(Parameter.Choose(flagConfig, flagNames));
                result.Add(Parameter.Check(valueConfig));
                result.Add(Parameter.JustDescrition("и наоборот"));
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
