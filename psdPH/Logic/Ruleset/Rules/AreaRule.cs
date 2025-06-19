using psdPH.Logic.Compositions;
using psdPH.Utils.Setups;
using System.Linq;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class AreaRule : LayerRule
    {
        [XmlIgnore]
        public AreaLeaf AreaLeaf {
            get => LayerComposition as AreaLeaf;
            set => LayerComposition = value;
        }

        public AreaRule(Composition composition) : base(composition) { }
    }
    public class AreaRuleSetupSource : LayerRuleSetupSource
    {
        protected Setup getAreaLeafSetup(object obj)
        {
            return getLayerCompositionSetup<AreaLeaf>(obj as LayerRule, "для зоны");
        }
    }


}
