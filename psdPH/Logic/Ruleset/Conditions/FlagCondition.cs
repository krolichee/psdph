using psdPH.Logic.Parameters;
using System;
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
        bool predicate(Parameter p) => p.Name == FlagName && p is FlagParameter;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                Parameter[] flagLeaves = Composition.ParameterSet.AsCollection().ToArray();
                var flagConfig = new SetupConfig(this, nameof(this.FlagParameter), "");
                var valueConfig = new SetupConfig(this, nameof(this.Value),"установлено в");
                result.Add(Setup.Choose(flagConfig, flagLeaves));
                result.Add(Setup.Check(valueConfig));
                return result.ToArray();
            }
        }

        [XmlIgnore]
        public FlagParameter FlagParameter
        {
            protected get
            {
                return Composition.ParameterSet.AsCollection().FirstOrDefault(predicate) as FlagParameter;
            }
            set
            {
                FlagName = value.Name;
            }
        }
        public override bool IsValid()
        {
            return FlagParameter.Toggle == Value;
        }
        public FlagCondition(Composition composition) : base(composition) { }
        public FlagCondition() : base(null) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&FlagName!=null;
        }
    }
    
}
