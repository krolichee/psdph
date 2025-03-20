using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Logic
{
    public static class PhotoshopLayerExtension
    {
        public static void Translate (this ArtLayer layer,Vector vector)
        {
            layer.Translate(vector.X,vector.Y);
        }
        public static Size GetBoundsSize(this ArtLayer layer)
        {
            var width = layer.Bounds[2] - layer.Bounds[0];
            var height = layer.Bounds[3] - layer.Bounds[1];
            return new Size(width,height);
        }
        public static void OffStyle(this ArtLayer layer)
        {
            layer.Application.DoAction("offFx", "psdPH");
        }
        public static void OnStyle(this ArtLayer layer)
        {
            layer.Application.DoAction("onFx", "psdPH");
        }
    }
}
