using Photoshop;
using psdPH.Photoshop;
using System;
using System.Linq;
using System.Windows;
using static psdPH.Logic.PhotoshopLayerExtension;
using Application = Photoshop.Application;

namespace psdPH.Logic
{
    public enum LayerListing
    {
        Recursive,
        OnlyHere
    }
    public static partial class PhotoshopDocumentExtension
    {
        public static void Chtoto(this ArtLayer artLayer)
        {
            var comp = artLayer.Application.ActiveDocument.LayerComps.Count;
            artLayer.Application.NotifiersEnabled = false;
           var n= artLayer.Application.Notifiers;
           // artLayer.Wrapper().OffStyle();
            Console.WriteLine();
        }
        
        public static void Rollback(this Document doc)
        {
            var initialState = doc.HistoryStates[1];
            doc.ActiveHistoryState = initialState;
        }
        const LayerListing DefaultListing = LayerListing.Recursive;
        public static Vector GetAlightmentVector(this Document doc, string targetLayerName, string dynamicLayerName, AlignOptions options)
        {
            ArtLayerWr targetLayer = new ArtLayerWr(doc.GetLayerByName(targetLayerName));
            ArtLayerWr dynamicLayer = new ArtLayerWr(doc.GetLayerByName(dynamicLayerName));
            return dynamicLayer.GetAlightmentVector(targetLayer, options);
        }
        public static ArtLayer CloneSmartLayer(this Document doc, string layername)
        {
            var layer = GetLayerByName(doc, layername);
            doc.ActiveLayer = layer;
            var psApp = doc.Application;
            psApp.ActiveDocument = doc;
            psApp.DoAction("cloneSmartLayer", "psdPH");
            return doc.ActiveLayer;
        }
        public static void SaveDocument(this Document doc, string savePath)
        {
            doc.SaveAs(savePath);///,PsSaveOptions.psSaveChanges, true, PsExtensionType.psLowercase);
        }

        public static string GetDocPath(this Document doc)
        {
            return doc.FullName;
        }

        public static Document OpenSmartLayer(this Document doc, string layername, LayerListing listing = DefaultListing)
        {
            ArtLayer layer = doc.GetLayerByName(layername, listing);
            return doc.OpenSmartLayer(layer);
        }
        public static Document OpenSmartLayer(this Document doc, ArtLayer layer)
        {
            Application psApp = doc.Application;
            psApp.ActiveDocument = doc;
            doc.ActiveLayer = layer;
            psApp.DoAction("openSmartLayer", "psdPH");
            return psApp.ActiveDocument;
        }
    }
}
