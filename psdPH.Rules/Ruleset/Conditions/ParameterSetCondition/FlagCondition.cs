using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using psdPH.Utils.Setups;
using psdPH.Setups;
using psdPH.Parameters;

namespace psdPH.Logic.Rules
{
    [Obsolete]
    public class FlagCondition : CompositionCondition
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
                var flagConfig = new ReflectionConfig(this, nameof(this.FlagParameter), "");
                var valueConfig = new ReflectionConfig(this, nameof(this.Value),"установлено в");
                result.Add(new ChooseSetup(flagConfig, flagLeaves));
                result.Add(new CheckSetup(valueConfig));
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
            return FlagName!=null;
        }
    }
    
}
