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
using System.Windows.Media;

namespace psdPH.Logic
{
    public enum LayerListing
    {
        Recursive,
        OnlyHere
    }
    public static partial class PhotoshopDocumentExtension
    {
        public static void AlignLayer(this Document doc, ArtLayer targetLayer, ArtLayer dynamicLayer)
        {
            targetLayer.Translate(doc.GetAlightmentVector(targetLayer, dynamicLayer));
        }
        public static void FitTextLayer(ArtLayer textLayer, ArtLayer areaLayer)
        {
            bool isFits(Size fittable, Size area) => fittable.Width <= area.Width && fittable.Height <= area.Height;
            bool isFitsWithToler(Size fittable, Size area, int toler,out bool fits)
            {
                fits = isFits(fittable, area);
                double[] diffs = new double[] { area.Width - fittable.Width, area.Height - fittable.Height };
                return fits && (diffs.Min() <= toler) && isFits(fittable, area);
            }

            var areaSize = areaLayer.GetBoundsSize();
            double fontSizeShift = textLayer.TextItem.Size/2;

            bool _fits;

            while (!isFitsWithToler(textLayer.GetBoundsSize(), areaSize,3,out _fits))
            {
                if (_fits)
                    textLayer.TextItem.Size += fontSizeShift;
                else
                    textLayer.TextItem.Size -= fontSizeShift;
                fontSizeShift /= 2;
            }
        }
        public static void FitTextLayer(this Document doc, string textLayerName, string areaLayerName)
        {
            FitTextLayer(
                doc.GetLayerByName(textLayerName),
                doc.GetLayerByName(areaLayerName)
                );
        }
        public static void Rollback(this Document doc)
        {
            var initialState = doc.HistoryStates[1];
            doc.ActiveHistoryState = initialState;
        }
        const LayerListing DefaultListing = LayerListing.Recursive;
        public class Alignment
        {
            public override int GetHashCode()=> (int)H*4 + (int)V;
            public HorizontalAlignment H;
            public VerticalAlignment V;
            public Alignment(HorizontalAlignment horizontal,VerticalAlignment vertical)
            {
                H = horizontal;
                V = vertical;
            }
        }
        Vector GetAlightmentVector(Rect targetRect, Rect dynamicRect)
        {
            
        }

        public static Vector GetAlightmentVector(this Document doc, ArtLayer targetLayer, ArtLayer dynamicLayer,Alignment alignment=null)
        {
            if (alignment == null)
                alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);
            int x = 0;
            switch (alignment.H)
            {
                case HorizontalAlignment.Left:
                    x = targetLayer.Bounds[0] - dynamicLayer.Bounds[0];
                    break;
                case HorizontalAlignment.Right:
                    x = targetLayer.Bounds[2] - dynamicLayer.Bounds[2];
                    break;
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    double t_w = targetLayer.GetBoundsSize().Width;
                    double d_w = dynamicLayer.GetBoundsSize().Width;
                    x = (targetLayer.Bounds[0]+t_w) - (dynamicLayer.Bounds[0]+d_w);
                    break;
            }
            switch (alignment.V)
            {
                case VerticalAlignment.Top:
                    x = targetLayer.Bounds[1] - dynamicLayer.Bounds[1];
                    break;
                case VerticalAlignment.Bottom:
                    x = targetLayer.Bounds[3] - dynamicLayer.Bounds[3];
                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    double t_h = targetLayer.GetBoundsSize().Height;
                    double d_h = dynamicLayer.GetBoundsSize().Height;
                    x = (targetLayer.Bounds[1]+t_h) - (dynamicLayer.Bounds[1]+d_h);
                    break;
            }
            return new Vector(
                targetLayer.Bounds[0] - dynamicLayer.Bounds[0],
                targetLayer.Bounds[1] - dynamicLayer.Bounds[1]
                );
        }
        public static Vector GetAlightmentVector(this Document doc, string targetLayerName, string dynamicLayerName)
        {
            ArtLayer targetLayer = doc.GetLayerByName(targetLayerName);
            ArtLayer dynamicLayer = doc.GetLayerByName(dynamicLayerName);
            return doc.GetAlightmentVector(targetLayer, dynamicLayer);
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
