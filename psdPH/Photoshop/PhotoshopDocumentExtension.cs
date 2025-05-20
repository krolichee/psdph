using Photoshop;
using System.Linq;
using System.Windows;
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
        
        public static void Rollback(this Document doc)
        {
            var initialState = doc.HistoryStates[1];
            doc.ActiveHistoryState = initialState;
        }
        const LayerListing DefaultListing = LayerListing.Recursive;
        public class Alignment
        {
            public override int GetHashCode() => (int)H * 4 + (int)V;
            public HorizontalAlignment H;
            public VerticalAlignment V;
            public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
            {
                H = horizontal;
                V = vertical;
            }
        }
        public static Vector GetAlightmentVector(this Document doc, string targetLayerName, string dynamicLayerName, Alignment alignment = null)
        {
            ArtLayer targetLayer = doc.GetLayerByName(targetLayerName);
            ArtLayer dynamicLayer = doc.GetLayerByName(dynamicLayerName);
            return dynamicLayer.GetAlightmentVector(targetLayer);
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
            doc.SaveAs(savePath, new PsSaveOptions(), true, PsExtensionType.psLowercase);
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
