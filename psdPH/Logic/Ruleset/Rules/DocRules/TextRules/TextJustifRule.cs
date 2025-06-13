using Photoshop;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class TextJustifRule : TextRule, DocRule
    {
        public PsJustification Justification;
        public TextJustifRule(Composition composition) : base(composition) { }
        public TextJustifRule() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = base.Setups.ToList();
                var justificationConfig = new SetupConfig(this, nameof(this.Justification), "установить");
                result.Add(Setup.Choose(justificationConfig, new PsJustification[] {
                    PsJustification.psRight,
                    PsJustification.psLeft,
                    PsJustification.psCenter
                }.Cast<object>().ToArray(), FieldFunctions.EnumWrapperFunctions));
                return result.ToArray();
            }
        }
        public override string ToString() => "выравнивание шрифта";

        protected override void _apply(Document doc)
        {
            doc.GetLayerByName(LayerName).TextItem.Justification = Justification;
        }
    }

}
