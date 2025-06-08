using Photoshop;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Зона")]
    public class AreaLeaf : LayerComposition
    {
        [XmlIgnore]
        public override Setup[] Setups => new Setup[0];
        public override void Apply(Document doc) {
            ArtLayerWr(doc).Visible = false;
        }

        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Layer(LayerName).DoesDocHas(doc);
        }
    }

}

