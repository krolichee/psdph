using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using psdPH.Logic.Ruleset.Conditions;
using System.Xml.Serialization;
using psdPH.Setups;

namespace psdPH.Nodes
{
    public class ConditionNode : Node
    {
        [XmlIgnore]
        Condition Condition;
        [XmlIgnore]
        public bool Output;

        public ConditionNode(Condition condition)
        {
            Condition = condition;
        }

        public ConditionNode() { }

        [XmlIgnore]
        public Setup OutputSetup => Setup.Sealed(new ReflectionConfig(this,nameof(Output),"результат"));
        [XmlIgnore]
        public override List<Setup> Inputs => Condition.Setups.ToList();
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>() { OutputSetup };
        public override string ToString()=> Localization.LocalizationService.Localize(Condition);
        protected override void _apply()
        {
            Output = Condition.IsValid();
        }
    }
}
