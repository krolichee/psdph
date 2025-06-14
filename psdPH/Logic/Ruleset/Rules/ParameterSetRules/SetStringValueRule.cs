using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules.ParameterSetRules
{
    public class SetStringValueRule : ParameterSetRule
    {
        public override string ToString() => "установить текст";
        public string Value;
        public SetStringValueRule() : base(null as Composition) { }
        public SetStringValueRule(Composition composition) : base(composition) { ParameterSet = composition.ParameterSet; }
        public SetStringValueRule(ParameterSet parameterSet) : base(parameterSet) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                var valueConfig = new SetupConfig(this, nameof(this.Value), "в");
                result.Add(getParameterSetup<StringParameter>(nameof(StringParameter), ""));
                result.Add(Setup.StringInput(valueConfig));
                return result.ToArray();
            }
        }
        public void CoreApply()
        {
            _apply(null);
        }
        public string FlagName;
        bool predicate(Parameter p) => p.Name == FlagName && p is FlagParameter;
        [XmlIgnore]
        public StringParameter StringParameter
        {
            protected get
            {
                var parset = ParameterSet == null ? Composition.ParameterSet : ParameterSet;
                return parset.AsCollection().FirstOrDefault(predicate) as StringParameter;
            }
            set
            {
                FlagName = value.Name;
            }
        }
        protected override void _apply(Document doc) =>
            StringParameter.Text = Value;
        public override bool IsSetUp()
        {
            return base.IsSetUp() && FlagName != null;
        }
    }
}
