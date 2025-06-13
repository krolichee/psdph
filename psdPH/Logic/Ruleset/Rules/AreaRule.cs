using psdPH.Logic.Compositions;
using System.Linq;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class AreaRule : LayerRule
    {
        protected Setup[] getAlignOptionsParameters()
        {
            var alingment_config = new SetupConfig(this, nameof(Alignment), "с выравниванием");
            var considerfx_config = new SetupConfig(this, nameof(ConsiderFx), "по границам");

            return new Setup[]{
            Setup.AlignmentInput(alingment_config),
            Setup.EnumChoose(considerfx_config,typeof(ConsiderFx))
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
        protected Setup getAreaParameter()
        {
            var layerNameConfig = new SetupConfig(this, nameof(this.AreaLayerName), "по зоне");
            var layerNames = Composition.GetChildren<AreaLeaf>().Select(a => a.LayerName).ToArray();
            return Setup.Choose(layerNameConfig, layerNames);
        }
        protected Setup[] getLayerAndAreaParameters()
        {
            return new Setup[] { getLayerParameter(), getAreaParameter() };
        }

        [XmlIgnore]
        public AreaLeaf AreaLeaf
        {
            protected get => Composition.GetChildren<AreaLeaf>().First((c) => c.LayerName == AreaLayerName); set
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
