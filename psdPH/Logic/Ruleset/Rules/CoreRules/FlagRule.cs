using Photoshop;
using psdPH.Logic.Parameters;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public class FlagRule : ConditionRule, ParameterSetRule
    {
        public override string ToString() => "значение флага";
        public ParameterSet ParameterSet;
        public string FlagName;
        public bool Value=true;
        public FlagRule() : base(null) { }
        public FlagRule(Composition composition) : base(composition) { }
        bool predicate(Parameter p) => p.Name == FlagName && p is FlagParameter;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                FlagParameter[] flagParameters = Composition.ParameterSet.GetByType<FlagParameter>().ToArray();
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
            FlagParameter.Value = Condition.IsValid()==Value;
        }
        [XmlIgnore]
        public FlagParameter FlagParameter
        {
            protected get
            {
                var parset = ParameterSet == null ? Composition.ParameterSet : ParameterSet;
                return parset.AsCollection().FirstOrDefault(predicate) as FlagParameter;
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

        public void SetParameterSet(ParameterSet parset)
        {
            ParameterSet = parset;
            if (Condition is ParameterSetCondition)
                (Condition as ParameterSetCondition).SetParameterSet(parset);
        }
    }

}
