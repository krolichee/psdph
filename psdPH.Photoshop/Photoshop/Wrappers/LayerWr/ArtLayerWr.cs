using Photoshop;

namespace psdPH.Photoshop
{
    public partial class ArtLayerWr : LayerWr
    {
        private ArtLayer _layer;
        public ArtLayer ArtLayer { get => _layer; }
        public ArtLayerWr(ArtLayer layer) =>
            _layer = layer;

    }
}
