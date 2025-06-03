using Photoshop;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {
        
        public static void AdjustToWidth(this LayerWr layer, double width, ConsiderFx considerFx)
        {
            var bounds = layer.GetBoundRect(considerFx);
            var layerWidth = bounds.Width;
            if (layerWidth == 0)
                return;
            
            double resizeRatio = width / layerWidth * 100;
            layer.Resize(resizeRatio, resizeRatio);
        }
        public static void AdjustTo(this LayerWr layer, LayerWr areaLayer, AlignOptions options)
        {
            Rect getBounds(LayerWr l) => l.GetBoundRect(options.ConsiderFx);
            double getHeight(LayerWr l) => getBounds(l).Height;
            double getWidth(LayerWr l) => getBounds(l).Width;

            var layerHeight = getHeight(layer);
            var layerWidth = getWidth(layer);
            var areaHeight = getHeight(areaLayer);
            var areaWidth = getWidth(areaLayer);

            if (new double[] { areaWidth, areaHeight,layerWidth ,layerHeight }.Any(d=>d==0))
                return;

            double layerRatio = layerWidth / layerHeight;
            double areaRatio = areaWidth / areaHeight;
            double resizeRatio;
            void adjustByWidth()
            {
                resizeRatio = areaWidth / layerWidth * 100;
            }
            void adjustByHeight()
            {
                resizeRatio = areaHeight / layerHeight * 100;
            }
            if (layerRatio >= areaRatio)
                adjustByWidth();
            else
                adjustByHeight();
            layer.Resize(resizeRatio, resizeRatio);
        }
        
        public static void AdjustTextLayerToWidth(this ArtLayerWr textLayerWr, double width)
        {
            if (textLayerWr.GetBoundRect().Width == 0)
                return;
            textLayerWr.GroupLayer();
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

            while (!isFitsInWithToler(textLayerWr.GetBoundsSize().Width, width, 3, out bool _fits))
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

        public static LayerSetWr EqualizeLineWidth(this TextLayerWr textLayerWr)
        {
           
            LayerSetWr lineLayerSetWr = textLayerWr.SplitTextLayer();
            ArtLayerWr[] lineLayers = lineLayerSetWr.ArtLayers.Cast<ArtLayer>().Select(l=>new ArtLayerWr(l)).ToArray();
            double maxWidth = lineLayers.Max((l) => l.GetBoundRect().Width);

            List<double> prevLineGaps = new List<double> { 0 };
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                ArtLayerWr layer = lineLayers[i];
                ArtLayerWr prevLayer = lineLayers[i - 1];
                prevLineGaps.Add(layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom);
            }
            lineLayers[0].AdjustToWidth(maxWidth,ConsiderFx.NoFx);
            for (int i = 1; i < lineLayers.Count(); i++)
            {
                double prevLineGap = prevLineGaps[i];
                ArtLayerWr layer = lineLayers[i];
                ArtLayerWr prevLayer = lineLayers[i - 1];
                layer.AdjustToWidth(maxWidth, ConsiderFx.NoFx);
                double curGap = layer.GetBoundRect().Top - prevLayer.GetBoundRect().Bottom;
                layer.TranslateV(new Vector(0, prevLineGap - curGap));
            }
            return lineLayerSetWr;
        }
        public static void FitWithEqualize(this TextLayerWr textLayerWr, ArtLayerWr areaLayer, AlignOptions options)
        {
            bool hasStyle = textLayerWr.HasStyle();
            if(hasStyle)
            {
                textLayerWr.CopyStyle();
                textLayerWr.OffStyle();
            }
            LayerSetWr equalizedWr = textLayerWr.EqualizeLineWidth();
            if (hasStyle)
                equalizedWr.PasteStyle();
            equalizedWr.Fit(areaLayer, options);
            
        }
        public static void Fit(this LayerWr layerWr, ArtLayerWr areaLayer, AlignOptions options)
        {
            layerWr.AdjustTo(areaLayer, options);
            layerWr.AlignLayer(areaLayer, options);
        }

        //public static void AdjustTextLayerTo(this ArtLayerWr textLayer, ArtLayerWr areaLayer)
        //{
        //    bool isFitsIn(Size fittable, Size area) => fittable.Width <= area.Width && fittable.Height <= area.Height;
        //    bool isFitsInWithToler(Size fittable, Size area, int toler, out bool fits)
        //    {
        //        fits = isFitsIn(fittable, area);
        //        if (!fits)
        //            return false;
        //        double[] diffs = new double[] { area.Width - fittable.Width, area.Height - fittable.Height };
        //        return diffs.Min() <= toler;
        //    }
        //    var textItem = textLayer.ArtLayer.TextItem;
        //    var areaSize = areaLayer.GetBoundsSize();
        //    double fontSizeShift = textItem.Size / 2;

        //    bool _fits;

        //    while (!isFitsInWithToler(textLayer.GetBoundsSize(), areaSize, 3, out _fits))
        //    {
        //        if (_fits)
        //            textItem.Size += fontSizeShift;
        //        else
        //            textItem.Size -= fontSizeShift;
        //        fontSizeShift /= 2;
        //        if (fontSizeShift <= 0.5)
        //            break;
        //    }
        //}
    }
}
