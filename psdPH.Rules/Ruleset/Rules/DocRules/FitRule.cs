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
        public override void Apply(DocumentWr doc)
        {
            LayerWr layer = getLayerWr(doc);
            ArtLayerWr area = AreaLeaf.GetLayerWr(doc) as ArtLayerWr;
            if (BalanceFont && layer is TextLayerWr)
                (layer as TextLayerWr).FitWithEqualize(area, AlignOptions);
            else
                layer.Fit(area, AlignOptions);
        }
    }

}
