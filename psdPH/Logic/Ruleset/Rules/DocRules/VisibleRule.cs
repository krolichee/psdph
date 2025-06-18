using Photoshop;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class VisibleRule : LayerRule, DocRule
    {
        public override string ToString() => "видимость";
        public bool Toggle = true;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var opacityConfig = new SetupConfig(this, nameof(this.Toggle), "установить");
                result.Add(getLayerParameter());
                result.Add(new CheckSetup(opacityConfig));
                result.Add(JustDescription.JustDescrition("и наоборот"));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            getRuledLayerWr(doc).Visible = Toggle;
        }
        protected override void _else(Document doc)
        {
            getRuledLayerWr(doc).Visible = !Toggle;
        }
        public VisibleRule(Composition composition) : base(composition) { }
        public VisibleRule() : base(null) { }
    }

}
