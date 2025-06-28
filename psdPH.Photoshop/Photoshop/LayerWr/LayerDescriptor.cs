using Photoshop;
using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Photoshop
{
    public class LDFilter
    {
        Func<string, bool> layerNamePredicate;
        bool Kinded;
        PsLayerKind[] Kinds;
        bool? IsGroup;
        public LayerDescriptor[] Filter(LayerDescriptor[] lds)
        {
            bool kindFilter(LayerDescriptor ld) => 
                Kinded? 
                ld.Kind == null ? true : Kinds.Contains((PsLayerKind)ld.Kind)
                :true;
            bool layerNameFilter(LayerDescriptor ld) => (layerNamePredicate ?? ((_) => true))(ld.LayerName);
            bool isGroupFilter(LayerDescriptor ld) => IsGroup == null? true : ld.IsGroup == IsGroup;

            IEnumerable<LayerDescriptor> result = lds;
            result = result.Where(isGroupFilter);
            result = result.Where(kindFilter);
            result = result.Where(layerNameFilter);

            return result.ToArray();
        }
        public static LDFilter Group(Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, false, null, true);
        public static LDFilter Layer(PsLayerKind[] kinds, Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, true, kinds, false);
        public static LDFilter Layer(PsLayerKind kind, Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, true, new PsLayerKind[] {kind}, false);

        public static LDFilter Layer(Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, false, null, false);

        protected LDFilter(Func<string, bool> layerNamePredicate, bool kinded, PsLayerKind[] kinds, bool? isGroup)
        {
            this.layerNamePredicate = layerNamePredicate;
            Kinded = kinded;
            Kinds = kinds;
            IsGroup = isGroup;
        }
    }
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
        public LayerWr GetLayerWr(DocumentWr docWr)
        {
            LayerWr result;
            if (IsGroup)
                result = GetLayerSetWr(docWr.Doc);
            else
                result = GetArtLayerWr(docWr.Doc);
            return result;
        }
        public bool DoesDocHas(DocumentWr docWr)
        {
            try
            {
                GetLayerWr(docWr);
                return true;
            }
            catch { return false; }
        }
        public override string ToString() => LayerName;
        static LayerDescriptor GetLayerDescriptor(ArtLayer layer)
        {
            return LayerDescriptor.Layer(layer.Name, layer.Kind);
        }
        static LayerDescriptor GetLayerDescriptor(LayerSet layer)
        {
            return LayerDescriptor.Group(layer.Name);
        }
        public static LayerDescriptor[] GetLayerDescriptors(DocumentWr docWr)
        {
            var result = new List<LayerDescriptor>();
            var artLayers = docWr.Doc.GetArtLayers();
            var layerSets = docWr.Doc.GetLayerSets();
            result.AddRange(artLayers.Select(al => GetLayerDescriptor(al)));
            result.AddRange(layerSets.Select(ls => GetLayerDescriptor(ls)));
            return result.ToArray();
        }
        public static LayerDescriptor Group(string layername)=> new LayerDescriptor() 
        { IsGroup = true, LayerName = layername };
        public static LayerDescriptor Layer(string layername) => new LayerDescriptor()
        { IsGroup = false, LayerName = layername };
        public static LayerDescriptor Layer(string layername,PsLayerKind kind) => new LayerDescriptor()
        { IsGroup = false, LayerName = layername,Kind = kind};
    }


}

