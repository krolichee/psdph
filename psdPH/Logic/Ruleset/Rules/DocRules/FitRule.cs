using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.Utils.Setups;
using System.Collections.Generic;

namespace psdPH.Logic.Ruleset.Rules
{
    public class FitRule : AlignRule
    {
        public override string ToString() => "вместить";
        public bool BalanceFont = false;
        public FitRule(Composition composition) : base(composition) { }
        public FitRule() : base(null) {
            SetupsRegistry.Register<FitRule>(new AlignRuleSetupSource());
            DtoConvertersRegistry.Register<FitRule>(new AlignRuleDtoConverter());
        }
        public override void Apply(Document doc)
        {
            LayerWr layer = getRuledLayerWr(doc);
            ArtLayerWr area = AreaLeaf.ArtLayerWr(doc);
            if (BalanceFont && layer is TextLayerWr)
                (layer as TextLayerWr).FitWithEqualize(area, AlignOptions);
            else
                layer.Fit(area, AlignOptions);
        }
    }

}
