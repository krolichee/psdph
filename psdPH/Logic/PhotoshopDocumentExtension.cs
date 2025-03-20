using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Application = Photoshop.Application;
using psdPH.Logic;
using psdPH.Logic.Rules;

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
        public static Point GetRelativeLayerShift(this Document doc, ArtLayer currentLayer, ArtLayer relativeLayer)
        {
            return new Point(
                currentLayer.Bounds[0] - relativeLayer.Bounds[0],
                currentLayer.Bounds[1] - relativeLayer.Bounds[1]
                );
        }
        public static Point GetRelativeLayerShift(this Document doc, string currentLayerName, string relativeLayerName)
        {
            ArtLayer currentLayer = doc.GetLayerByName(currentLayerName);
            ArtLayer relativeLayer = doc.GetLayerByName(relativeLayerName);
            return doc.GetRelativeLayerShift(currentLayer, relativeLayer);
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
