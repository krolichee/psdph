using Photoshop;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

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
                    layers.AddRange(layerSet.GetArtLayers(listing));
            return layers.ToArray();
        }
        public static ArtLayer[] GetArtLayers(this LayerSet layerSet, LayerListing listing = DefaultListing)
        {
            List<ArtLayer> layers = new List<ArtLayer>();
            layers.AddRange(layerSet.ArtLayers.Cast<ArtLayer>().ToArray());
            if (listing == LayerListing.Recursive)
                foreach (LayerSet nestlayerSet in layerSet.LayerSets)
                    layers.AddRange(nestlayerSet.GetArtLayers(listing));
            return layers.ToArray();
        }
        //private static void ProcessLayerSet(LayerSet layerSet, List<ArtLayer> layers)
        //{
        //    foreach (ArtLayer layer in layerSet.ArtLayers)
        //        layers.Add(layer);
        //    foreach (LayerSet nestedLayerSet in layerSet.LayerSets)
        //        ProcessLayerSet(nestedLayerSet, layers);
        //}
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
                return kind == layer.Kind;
            }
            return GetArtLayers(doc, listing).Where(filter).ToArray();
        }

        private static ArtLayer FindLayerById(this Document doc, int layerId, LayerListing listing = DefaultListing)
        {
            ArtLayer[] layers = doc.GetArtLayers(listing);
            return layers.FirstOrDefault(l => l.id == layerId);
        }
        public static ArtLayer GetLayerByName(this Document doc, string layerName, LayerListing listing = DefaultListing)
        {
            ArtLayer[] layers = doc.GetArtLayers(listing);
            return layers.First(l => l.Name == layerName);
        }
        public static void ResetHistory(this Document doc)
        {
            doc.ActiveHistoryState = doc.HistoryStates[1];
        }
        public static bool IsNonFile(this Document doc)
        {
            try
            {
                doc.GetDocPath();
                return false;
            }
            catch {
                return true;
            }
        }
        [Obsolete]
        public static void FixTextLayersNames(this Document doc)
        {
            var layers = doc.GetLayersByKind(PsLayerKind.psTextLayer);
            foreach (var layer in layers)
            {
                var name = layer.Name;
                layer.Name = "_";
                layer.Name = name;
            }
            var smartLayers = doc.GetLayersByKind(PsLayerKind.psSmartObjectLayer);
            foreach (var layer in smartLayers)
            {
                var smart_doc = doc.OpenSmartLayer(layer);
                smart_doc.FixTextLayersNames();
                smart_doc.Close(PsSaveOptions.psSaveChanges);
            }

        }
        public static LayerWr GetLayerWrByName(this Document doc, string layerName, LayerListing listing = DefaultListing) {
            LayerWr result;
            try
            {

                var layer = doc.GetLayerByName(layerName, listing);
                try
                {
                    result =layer.TextWrapper();
                }
                catch
                {
                    result = layer.Wrapper();
                }
            }
            catch
            {
                result= doc.GetLayerSetByName(layerName, listing).Wrapper();
            }
            return result;
        }
    }
}
