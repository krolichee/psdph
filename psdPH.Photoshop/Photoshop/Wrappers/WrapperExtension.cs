using Photoshop;

namespace psdPH.Photoshop
{
    public static class WrapperExtension
    {
        public static ArtLayerWr Wrapper(this ArtLayer layer) => new ArtLayerWr(layer);
        public static LayerSetWr Wrapper(this LayerSet layer) => new LayerSetWr(layer);
        public static TextLayerWr TextWrapper(this ArtLayer layer) => new TextLayerWr(layer);
        public static DocumentWr Wrapper(this Document document) => new DocumentWr(document);
    }
}
