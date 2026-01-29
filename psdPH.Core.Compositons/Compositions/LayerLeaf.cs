using Photoshop;
using psdPH.Photoshop;
using psdPH.Utils;
using psdPH.Utils.Setups;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Слой")]
    public class LayerLeaf : LayerComposition
    {
        public override void Apply(DocumentWr doc) { }

        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.Layer(LayerName).IsInDoc(doc);
        }
    }

}

