using Photoshop;
using psdPH.Logic.Compositions;
using System.Collections.Generic;

namespace psdPH.Logic
{
    public class AlignRule : AreaRule
    {
        
        public override string ToString() => "выровнять";
        public AlignRule(Composition composition) : base(composition) { }
        public AlignRule() : base(null) { }
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>() { };
                result.AddRange(getLayerAndAreaParameters());
                result.AddRange(getAlignOptionsParameters());
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            getRuledLayerWr(doc).AlignLayer(AreaLeaf.ArtLayerWr(doc), AlignOptions);
        }

    }

}
