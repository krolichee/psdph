using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public static partial class PhotoshopDocumentExtension
    {
        public static string[] GetLayersNames(ArtLayer[] layers)
        {
            return layers.Select(l => l.Name).ToArray();
        }
        public static string[] GetLayersNames(this Document doc, ArtLayer[] layers)
        {
            return GetLayersNames(layers);
        }
        public static string[] GetLayersNames(this Document doc, LayerListing listing = DefaultListing)
        {
            return GetArtLayers(doc, listing).Select(l => l.Name).ToArray();
        }
        public static ArtLayer[] GetArtLayers(this Document doc, LayerListing listing = DefaultListing)
        {

            List<ArtLayer> layers = new List<ArtLayer>();
            foreach (ArtLayer item in doc.ArtLayers)
                layers.Add(item);

            if (listing == LayerListing.Recursive)
                foreach (LayerSet layerSet in doc.LayerSets)
                    ProcessLayerSet(layerSet, layers);

            return layers.ToArray();
        }
        private static void ProcessLayerSet(LayerSet layerSet, List<ArtLayer> layers)
        {


            foreach (ArtLayer layer in layerSet.ArtLayers)
                layers.Add(layer);
            foreach (LayerSet nestedLayerSet in layerSet.LayerSets)
                ProcessLayerSet(nestedLayerSet, layers);
        }
        public static ArtLayer[] GetLayersByKinds(this Document doc, PsLayerKind[] kinds, LayerListing listing = DefaultListing)
        {
            bool filter(ArtLayer layer)
            {
                return kinds.Contains(layer.Kind);
            }
            return GetArtLayers(doc, listing).Where(filter).ToArray();
        }
        public static ArtLayer[] GetLayersByKind(this Document doc, PsLayerKind kind, LayerListing listing = DefaultListing)
        {
            bool filter(ArtLayer layer)
            {
                return kind==layer.Kind;
            }
            return GetArtLayers(doc, listing).Where(filter).ToArray();
        }

        private static ArtLayer FindLayerById(this Document doc, int layerId, LayerListing listing = DefaultListing)
        {
            ArtLayer[] layers = doc.GetArtLayers(listing);
            return layers.First(l => l.id == layerId);
        }
        public static ArtLayer GetLayerByName(this Document doc, string layerName, LayerListing listing = DefaultListing)
        {
            ArtLayer[] layers = doc.GetArtLayers(listing);
            return layers.First(l => l.Name == layerName);
        }
    }
}
