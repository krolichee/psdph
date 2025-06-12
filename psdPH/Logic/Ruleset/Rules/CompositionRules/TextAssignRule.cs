using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Ruleset.Rules.CompositionRules;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public class TextAssignRule : TextRule, CompositionRule
    {
        public string StringName;
        bool predicate(Parameter p) => p.Name == StringName && p is StringParameter;
        [XmlIgnore]
        public StringParameter StringParameter
        {
            protected get
            {
                return Composition.ParameterSet.AsCollection().FirstOrDefault(predicate) as StringParameter;
            }
            set
            {
                StringName = value?.Name;
            }
        }
        public TextAssignRule(Composition composition) : base(composition) { }
        public TextAssignRule() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                Parameter[] stringParameters = Composition.ParameterSet.GetByType<StringParameter>().ToArray();
                var stringConfig = new SetupConfig(this, nameof(this.StringParameter), "из");
                result.Add(getTextLeafSetup());
                result.Add(Setup.Choose(stringConfig, stringParameters));
                return result.ToArray();
            }
        }
        public override string ToString() => "установить текст";

        protected override void _apply(Document doc)
        {
            TextLeaf.Text = StringParameter.Text;
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp() && StringName != null;
        }

        public void CompApply()
        {
            Apply(null);
        }
    }

}
