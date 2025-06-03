using Photoshop;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    public class OpacityRule : LayerRule
    {
        protected static int notSetOpacity => -1;
        public override string ToString() => "прозрачность";

        public int Opacity=notSetOpacity;
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var opacityConfig = new ParameterConfig(this, nameof(this.Opacity), "установить");
                result.Add(getLayerParameter());
                result.Add(Parameter.IntInput(opacityConfig, 0, 100));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            dynamic layer = getRuledLayerWr(doc);
            layer.Opacity = Opacity;
        }
        public OpacityRule(Composition composition) : base(composition) { }
        public OpacityRule() : base(null) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&Opacity!=notSetOpacity;
        }
    }

}
