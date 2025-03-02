using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Logic
{
    public enum LayerListing
    {
        Recursive,
        OnlyHere
    }
    public static class PhotoshopDocumentExtension
    {
        const LayerListing DefaultListing = LayerListing.Recursive;

        public static void SaveDocument(this Document doc, string savePath)
        {
            doc.SaveAs(savePath, new PsSaveOptions(), true, PsExtensionType.psLowercase);
        }
        // instance usage
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

        private static void ProcessLayerSet(dynamic layerSet, List<ArtLayer> layers)
        {
            foreach (ArtLayer layer in layerSet.ArtLayers)
            {

                layers.Add(layer);
            }

            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                ProcessLayerSet(nestedLayerSet, layers);
            }
        }
        public static string GetDocPath(this Document doc)
        {
            return doc.FullName;
        }
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

        public static ArtLayer[] GetLayersByKinds(this Document doc, PsLayerKind[] kinds, LayerListing listing = DefaultListing)
        {
            bool filter(ArtLayer layer)
            {
                return kinds.Contains(layer.Kind);
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
        public static Document OpenSmartLayer(this Document doc, string layername, LayerListing listing = DefaultListing )
        {
            ArtLayer layer = doc.GetLayerByName(layername, listing);
            return doc.OpenSmartLayer(layer);
        }
        public static Document OpenSmartLayer(this Document doc, ArtLayer layer)
        {
            Application psApp = doc.Application;
            psApp.ActiveDocument = doc;
            doc.ActiveLayer = layer;
            psApp.DoAction("b", "a");
            return psApp.ActiveDocument;
        }
    }
}
