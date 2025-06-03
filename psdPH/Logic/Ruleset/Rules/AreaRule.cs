using psdPH.Logic.Compositions;
using System.Linq;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Logic
{
    public abstract class AreaRule : LayerRule
    {
        protected Parameter[] getAlignOptionsParameters()
        {
            var alingment_config = new ParameterConfig(this, nameof(Alignment), "с выравниванием");
            var considerfx_config = new ParameterConfig(this, nameof(ConsiderFx), "по границам");

            return new Parameter[]{
            Parameter.AlignmentInput(alingment_config),
            Parameter.EnumChoose(considerfx_config,typeof(ConsiderFx))
            };
        }
        public string AreaLayerName;
        public Alignment Alignment;

        public ConsiderFx ConsiderFx;
        [XmlIgnore]
        public AlignOptions AlignOptions
        {
            get => new AlignOptions(Alignment, ConsiderFx); set
            {
                Alignment = value.Alignment;
                ConsiderFx = value.ConsiderFx;
            }
        }
        protected Parameter getAreaParameter()
        {
            var layerNameConfig = new ParameterConfig(this, nameof(this.AreaLayerName), "по зоне");
            var layerNames = Composition.getChildren<AreaLeaf>().Select(a => a.LayerName).ToArray();
            return Parameter.Choose(layerNameConfig, layerNames);
        }
        protected Parameter[] getLayerAndAreaParameters()
        {
            return new Parameter[] { getLayerParameter(), getAreaParameter() };
        }

        [XmlIgnore]
        public AreaLeaf AreaLeaf
        {
            protected get => Composition.getChildren<AreaLeaf>().First((c) => c.LayerName == AreaLayerName); set
            {
                AreaLayerName = value.LayerName;
            }
        }
        public AreaRule(Composition composition) : base(composition) { }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&Alignment!=null&&AreaLayerName!=null;
        }
    }

}
