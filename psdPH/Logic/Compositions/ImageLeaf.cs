using Photoshop;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Изображение")]
    public class ImageLeaf : LayerComposition
    {
        public string Path;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Apply(Document doc)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Layer(LayerName, PsLayerKind.psSmartObjectLayer).DoesDocHas(doc);
        }
    }

}

