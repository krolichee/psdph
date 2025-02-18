using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public enum LayerListing
    {
        Recursive,
        OnlyHere
    }
    public class PhotoshopDocumentWrapper
    {
        Document _doc;
        public PhotoshopDocumentWrapper(Document doc)
        {
            _doc = doc;
        }

        public void SaveDocument(string savePath)
        {
            _doc.SaveAs(savePath, new PsSaveOptions(), true, PsExtensionType.psLowercase);
        }

        public ArtLayer[] GetArtLayers(LayerListing listing = LayerListing.OnlyHere)
        {
            List<ArtLayer> layers = new List<ArtLayer>();
            foreach (ArtLayer item in _doc.ArtLayers)
                layers.Add(item);

            if (listing == LayerListing.Recursive)
                foreach (LayerSet layerSet in _doc.LayerSets)
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

        public string GetDocPath()
        {
            return _doc.FullName;
        }
        public static string[] GetLayersNames(ArtLayer[] layers)
        {
            return layers.Select(l => l.Name).ToArray();
        }
        public string[] GetLayersNames(LayerListing listing = LayerListing.OnlyHere)
        {
            return GetArtLayers(listing).Select(l => l.Name).ToArray();
        }

        public ArtLayer[] GetLayersByKinds(PsLayerKind[] kinds)
        {
            bool filter(ArtLayer layer)
            {
                return kinds.Contains(layer.Kind);
            }
            return GetArtLayers().Where(filter).ToArray();
        }

        private ArtLayer FindLayerById(int layerId, LayerListing listing = LayerListing.OnlyHere)
        {
            ArtLayer[] layers = this.GetArtLayers(listing);
            return layers.Where(l => l.id == layerId).ToArray()[0];
        }
        public ArtLayer GetLayerByName(string layerName, LayerListing listing = LayerListing.OnlyHere)
        {
            ArtLayer[] layers = GetArtLayers(listing);
            return layers.Where(l => l.Name == layerName).ToArray()[0];
        }
        public Document OpenSmartLayer(string layername)
        {
            ArtLayer layer = GetLayerByName(layername);
            return OpenSmartLayer(_doc,layer);
        }
        public static Document OpenSmartLayer(Document doc, ArtLayer layer)
        {
            Application psApp = doc.Application;
            psApp.ActiveDocument = doc;
            doc.ActiveLayer = layer;
            psApp.DoAction("b", "a");
            return psApp.ActiveDocument;
        }
    }
}
