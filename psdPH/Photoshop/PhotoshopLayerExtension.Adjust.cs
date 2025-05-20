using Photoshop;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {
        public static void AdjustToWidth(this LayerWr layer, double width)
        {
            double resizeRatio = width / layer.GetBoundRect().Width*100;
            layer.Resize(resizeRatio, resizeRatio);
        }
        public static void AdjustTo(this LayerWr layer, LayerWr areaLayer)
        {
            double layerRatio = layer.GetBoundRect().Width / layer.GetBoundRect().Height;
            double areaRatio = areaLayer.GetBoundRect().Width / areaLayer.GetBoundRect().Height;
            double resizeRatio;
            void adjustByWidth()
            {
                resizeRatio = areaLayer.GetBoundRect().Width / layer.GetBoundRect().Width * 100;
            }
            void adjustByHeight()
            {
                resizeRatio = areaLayer.GetBoundRect().Height / layer.GetBoundRect().Height * 100;
            }
            if (layerRatio >= areaRatio)
                adjustByWidth();
            else
                adjustByHeight();
            layer.Resize(resizeRatio, resizeRatio);
        }
        
        public static void AdjustTextLayerToWidth(this ArtLayerWr textLayerWr, double width)
        {
            LayerSet layerSet = textLayerWr.GroupLayer() ;
            TextItem textItem = textLayerWr.ArtLayer.TextItem;

            if (textLayerWr.GetBoundRect().Width == 0 || textLayerWr.GetBoundRect().Width == width)
                return;
            bool isFitsIn(double actual, double target) => textLayerWr.GetBoundRect().Width <= width;
            bool isFitsInWithToler(double actual, double target, double toler, out bool fits)
            {
                fits = isFitsIn(actual, target);
                double diff = target - actual;
                if (!fits)
                    return false;
                return (diff <= toler);
            }
            double fontSizeShift = textItem.Size / 2;
            bool _fits;

            while (!isFitsInWithToler(textLayerWr.GetBoundsSize().Width, width, 3, out _fits))
            {
                if (_fits)
                    textItem.Size += fontSizeShift;
                else
                    textItem.Size -= fontSizeShift;
                fontSizeShift /= 2;
                if (fontSizeShift <= 0.5)
                    break;
            }
        }

        public static LayerSet EqualizeLineWidth(this ArtLayerWr textLayer)
        {
            LayerSet lineLayerSet = textLayer.SplitTextLayer();
            ArtLayerWr[] lineLayers = lineLayerSet.ArtLayers.Cast<ArtLayer>().Select(l=>new ArtLayerWr(l)).ToArray();
            double maxWidth = lineLayers.Max((l) => l.GetBoundRect().Width);

            List<double> prevLineGaps = new List<double> { 0 };
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                ArtLayerWr layer = lineLayers[i];
                ArtLayerWr prevLayer = lineLayers[i - 1];
                prevLineGaps.Add(layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom);
            }
            lineLayers[0].AdjustToWidth(maxWidth);
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                double prevLineGap = prevLineGaps[i];
                ArtLayerWr layer = lineLayers[i];
                ArtLayerWr prevLayer = lineLayers[i - 1];
                layer.AdjustToWidth(maxWidth);
                double curGap = layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom;
                layer.TranslateV(new Vector(0, prevLineGap - curGap));
            }
            return lineLayerSet;
        }
        public static void FitWithEqualize(this ArtLayerWr textLayer, ArtLayerWr areaLayer)
        {
            LayerSet equalized = textLayer.EqualizeLineWidth();
            LayerSetWr equalizedWr = new LayerSetWr(equalized);
            equalizedWr.AdjustTo(areaLayer);
            equalizedWr.AlignLayer(areaLayer, new Alignment(HorizontalAlignment.Center, VerticalAlignment.Center));
            equalizedWr.OnStyle();
        }

        public static void AdjustTextLayerTo(this ArtLayerWr textLayer, ArtLayerWr areaLayer)
        {
            bool isFitsIn(Size fittable, Size area) => fittable.Width <= area.Width && fittable.Height <= area.Height;
            bool isFitsInWithToler(Size fittable, Size area, int toler, out bool fits)
            {
                fits = isFitsIn(fittable, area);
                if (!fits)
                    return false;
                double[] diffs = new double[] { area.Width - fittable.Width, area.Height - fittable.Height };
                return diffs.Min() <= toler;
            }
            var textItem = textLayer.ArtLayer.TextItem;
            var areaSize = areaLayer.GetBoundsSize();
            double fontSizeShift = textItem.Size / 2;

            bool _fits;

            while (!isFitsInWithToler(textLayer.GetBoundsSize(), areaSize, 3, out _fits))
            {
                if (_fits)
                    textItem.Size += fontSizeShift;
                else
                    textItem.Size -= fontSizeShift;
                fontSizeShift /= 2;
                if (fontSizeShift <= 0.5)
                    break;
            }
        }
    }
}
