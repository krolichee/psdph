using Photoshop;
using psdPH.Photoshop;

namespace psdPH.Logic.Compositions
{
    public abstract class LayerComposition : Composition
    {
        public string LayerName;
        public override string ObjName => LayerName;
        public ArtLayerWr ArtLayerWr(Document doc)
        {
            return doc.GetLayerByName(LayerName).Wrapper();
        }
        public LayerComposition(string layername) { LayerName = layername; }
        public LayerComposition() { LayerName = string.Empty; }
        protected LayerWr getLayerWr(Document doc, string layerName) => doc.GetLayerWrByName(layerName);
    }
    public enum BlobMode
    {
        Layer,
        Path
    }

}

