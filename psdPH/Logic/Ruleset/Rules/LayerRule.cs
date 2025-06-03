using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    public abstract class LayerRule : ConditionRule
    {
        public ChangeMode ChangeMode = ChangeMode.Abs;
        public string LayerName;
        [XmlIgnore]
        public LayerComposition LayerComposition
        {
            protected get => Composition.getChildren<TextLeaf>().First(t => t.LayerName == LayerName);
            set => LayerName = value?.LayerName;
        }
        protected Parameter getLayerParameter()
        {
            var layerNames = Composition.getChildren<LayerComposition>().Select(c => c.LayerName).ToArray();
            var layerNameConfig = new ParameterConfig(this, nameof(this.LayerName), "для слоя");
            return Parameter.Choose(layerNameConfig, layerNames);
        }
        protected LayerWr getRuledLayerWr(Document doc) =>
            doc.GetLayerWrByName(LayerName);
        protected LayerRule(Composition composition) : base(composition) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&LayerName!=null;
        }
    };

}
