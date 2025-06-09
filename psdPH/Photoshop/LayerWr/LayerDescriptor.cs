using Photoshop;
using psdPH.Logic;
using psdPH.Photoshop;
using System.Linq;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH
{
    public class LayerDescriptor
    {
        public string LayerName;
        public PsLayerKind? Kind;
        public bool IsGroup;
        LayerSetWr GetLayerSetWr(Document doc) => doc.GetLayerSetByName(LayerName).Wrapper();
        ArtLayerWr GetArtLayerWr(Document doc)
        {
            ArtLayerWr result;
            if (Kind != null)
            {
                ArtLayer layer;
                layer = doc.GetLayersByKind((PsLayerKind)Kind).First(l => l.Name == LayerName);
                if (Kind == PsLayerKind.psTextLayer)
                    result = layer.TextWrapper();
                else
                    result = layer.Wrapper();
            }
            else
                result = doc.GetLayerByName(LayerName).Wrapper();
            return result;
        }
        public LayerWr GetFromDoc(Document doc)
        {
            LayerWr result;
            if (IsGroup)
                result = GetLayerSetWr(doc);
            else
                result = GetArtLayerWr(doc);
            return result;
        }
        public bool DoesDocHas(Document doc)
        {
            try
            {
                GetFromDoc(doc);
                return true;
            }
            catch { return false; }
        }
        public static LayerDescriptor Group(string layername)=> new LayerDescriptor() 
        { IsGroup = true, LayerName = layername };
        public static LayerDescriptor Layer(string layername) => new LayerDescriptor()
        { IsGroup = false, LayerName = layername };
        public static LayerDescriptor Layer(string layername,PsLayerKind kind) => new LayerDescriptor()
        { IsGroup = false, LayerName = layername,Kind = kind};
    }


}

