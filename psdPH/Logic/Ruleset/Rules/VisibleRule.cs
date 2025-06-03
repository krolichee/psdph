using Photoshop;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    public class VisibleRule : LayerRule
    {
        public override string ToString() => "видимость";
        public bool Toggle = true;
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var opacityConfig = new ParameterConfig(this, nameof(this.Toggle), "установить");
                result.Add(getLayerParameter());
                result.Add(Parameter.Check(opacityConfig));
                result.Add(Parameter.JustDescrition("и наоборот"));
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
