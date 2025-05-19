using Photoshop;
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
        public static void AdjustLayerToWidth(this ArtLayer layer, double width)
        {
            double resizeRatio = width / layer.GetBoundRect().Width*100;
            layer.Resize(resizeRatio, resizeRatio);
        }
        public static void AdjustLayerSetTo(this LayerSet layer, ArtLayer areaLayer)
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
        
        public static void AdjustTextLayerToWidth(this ArtLayer textLayer, double width)
        {
            LayerSet layerSet = textLayer.GroupLayer() ;

            if (textLayer.GetBoundRect().Width == 0 || textLayer.GetBoundRect().Width == width)
                return;
            bool isFitsIn(double actual, double target) => textLayer.GetBoundRect().Width <= width;
            bool isFitsInWithToler(double actual, double target, double toler, out bool fits)
            {
                fits = isFitsIn(actual, target);
                double diff = target - actual;
                if (!fits)
                    return false;
                return (diff <= toler);
            }
            double fontSizeShift = textLayer.TextItem.Size / 2;
            bool _fits;

            while (!isFitsInWithToler(textLayer.GetBoundsSize().Width, width, 3, out _fits))
            {
                if (_fits)
                    textLayer.TextItem.Size += fontSizeShift;
                else
                    textLayer.TextItem.Size -= fontSizeShift;
                fontSizeShift /= 2;
                if (fontSizeShift <= 0.5)
                    break;
            }
        }

        public static LayerSet EqualizeLineWidth(this ArtLayer textLayer)
        {
            LayerSet lineLayerSet = textLayer.SplitTextLayer();
            ArtLayer[] lineLayers = lineLayerSet.ArtLayers.Cast<ArtLayer>().ToArray();
            double maxWidth = lineLayers.Max((l) => l.GetBoundRect().Width);

            List<double> prevLineGaps = new List<double> { 0 };
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                ArtLayer layer = lineLayers[i];
                ArtLayer prevLayer = lineLayers[i - 1];
                prevLineGaps.Add(layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom);
            }
            lineLayers[0].AdjustLayerToWidth(maxWidth);
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                double prevLineGap = prevLineGaps[i];
                ArtLayer layer = lineLayers[i];
                ArtLayer prevLayer = lineLayers[i - 1];
                layer.AdjustLayerToWidth(maxWidth);
                double curGap = layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom;
                layer.TranslateV(new Vector(0, prevLineGap - curGap));
            }
            return lineLayerSet;
        }
        public static void FitWithEqualize(this ArtLayer textLayer, ArtLayer areaLayer)
        {
            LayerSet equalized = textLayer.EqualizeLineWidth();
            equalized.AdjustLayerSetTo(areaLayer);
            equalized.AlignLayer(areaLayer, new Alignment(HorizontalAlignment.Center, VerticalAlignment.Center));
            equalized.OnStyle();
        }

        public static void AdjustTextLayerTo(this ArtLayer textLayer, ArtLayer areaLayer)
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

            var areaSize = areaLayer.GetBoundsSize();
            double fontSizeShift = textLayer.TextItem.Size / 2;

            bool _fits;

            while (!isFitsInWithToler(textLayer.GetBoundsSize(), areaSize, 3, out _fits))
            {
                if (_fits)
                    textLayer.TextItem.Size += fontSizeShift;
                else
                    textLayer.TextItem.Size -= fontSizeShift;
                fontSizeShift /= 2;
                if (fontSizeShift <= 0.5)
                    break;
            }
        }
    }
}
