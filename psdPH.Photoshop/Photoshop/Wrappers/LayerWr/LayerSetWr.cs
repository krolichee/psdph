using Photoshop;

namespace psdPH.Photoshop
{
    public partial class LayerSetWr : LayerWr
    {
        private LayerSet _layer;
        public LayerSet LayerSet { get => _layer; }
        public ArtLayers ArtLayers => _layer.ArtLayers;
        public LayerSetWr(LayerSet layer) =>
            _layer = layer;
    }
}
