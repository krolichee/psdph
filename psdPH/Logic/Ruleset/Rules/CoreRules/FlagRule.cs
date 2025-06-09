using Photoshop;
using psdPH.Logic.Parameters;
using System;
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
        bool predicate(Parameter p) => p.Name == FlagName && p is FlagParameter;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                Parameter[] flagParameters = Composition.ParameterSet.ToArray();
                var flagConfig = new SetupConfig(this, nameof(this.FlagParameter), "");
                var valueConfig = new SetupConfig(this, nameof(this.Value), "установить в");
                result.Add(Setup.Choose(flagConfig, flagParameters));
                result.Add(Setup.Check(valueConfig));
                result.Add(Setup.JustDescrition("и наоборот"));
                return result.ToArray();
            }
        }
        public void CoreApply()
        {
            FlagParameter.Value = Condition.IsValid();
        }
        [XmlIgnore]
        public FlagParameter FlagParameter
        {
            protected get
            {
                return Composition.ParameterSet.FirstOrDefault(predicate) as FlagParameter;
            }
            set
            {
                FlagName = value.Name;
            }
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
