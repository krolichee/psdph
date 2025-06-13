using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using System.Collections.Generic;

namespace psdPH.Logic.Ruleset.Rules
{
    public class FitRule : AreaRule, DocRule
    {
        public override string ToString() => "вместить";
        public bool BalanceFont = false;
        public FitRule(Composition composition) : base(composition) { }
        public FitRule() : base(null) { }
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                result.AddRange(getLayerAndAreaParameters());
                var balance_config = new SetupConfig(this, nameof(BalanceFont), "балансировать шрифт");
                result.Add(Setup.Check(balance_config));
                result.AddRange(getAlignOptionsParameters());
                return result.ToArray();
            }

        }
        protected override void _apply(Document doc)
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
