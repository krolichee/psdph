using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {
        
        public static void AdjustTextLayerByWidth(this ArtLayer textLayer, double width)
        {
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
