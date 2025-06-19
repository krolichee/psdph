using Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Obsolete]
    [Serializable]
    [UIName("Изображение")]
    public class ImageLeaf : LayerComposition
    {
        public string Path;

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

