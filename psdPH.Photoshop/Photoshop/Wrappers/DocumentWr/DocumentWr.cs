using Photoshop;
using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Windows;
using Application = Photoshop.Application;

namespace psdPH.Photoshop
{
    public enum LayerListing
    {
        Recursive,
        OnlyHere
    }
    
    public class DocumentWr
    {
        Document doc;
        public Document Doc => doc;
        Application Application => doc.Application;
        const LayerListing DefaultListing = LayerListing.Recursive;

        public bool Saved => doc.Saved;

        public void Rollback()
        {
            var initialState = doc.HistoryStates[1];
            doc.ActiveHistoryState = initialState;
        }
        
        public DocumentWr(Document d)
        {
            doc = d;
        }
        public bool IsPathPresent(string path)
        {
            if (IsNonFile())
                return false;
            return GetDocPath() == path;
        }
        public Vector GetAlightmentVector(string targetLayerName, string dynamicLayerName, AlignOptions options)
        {
            ArtLayerWr targetLayer = new ArtLayerWr(doc.GetLayerByName(targetLayerName));
            ArtLayerWr dynamicLayer = new ArtLayerWr(doc.GetLayerByName(dynamicLayerName));
            return dynamicLayer.GetAlightmentVector(targetLayer, options);
        }
        public static DocumentWr[] GetDocs(Application psApp)
        {
            var result =new List<DocumentWr>();
            foreach (Document item in psApp.Documents)
            {
                result.Add(new DocumentWr(item));
            }
            return result.ToArray();
        }
        public void SaveDocument(string savePath)
        {
            doc.SaveAs(savePath);///,PsSaveOptions.psSaveChanges, true, PsExtensionType.psLowercase);
        }
        public void Close(PsSaveOptions options)
        {
            doc.Close(options);
        }
        public string GetDocPath()
        {
            if (IsNonFile())
                return null;
            return doc.FullName;
        }

        public DocumentWr OpenSmartLayer(LayerDescriptor layerD, LayerListing listing = DefaultListing)
        {
            ArtLayerWr layerWr = layerD.GetLayerWr(doc.Wrapper()) as ArtLayerWr;
            return OpenSmartLayer(layerWr).Wrapper();
        }
        public Document OpenSmartLayer(ArtLayerWr layerWr)
        {
            layerWr.Active = true;
            Application.DoAction("openSmartLayer", "psdPH");
            return Application.ActiveDocument;
        }
        public bool IsNonFile()
        {
            try
            {
                var _ = doc.FullName;
                return false;
            }
            catch
            {
                return true;
            }
        }

        public void Save()
        {
            doc.Save();
        }
    }
}
