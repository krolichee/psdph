using Photoshop;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;
using static psdPH.Logic.PhotoshopDocumentExtension;
using psdPH.Photoshop;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {
        public static Size GetBoundsSize(this ArtLayerWr layer)
        {
            return layer.GetBoundRect().Size;
        }
        public static LayerSet SplitTextLayer(this ArtLayerWr artLayerWr)
        {
            LayerSets parentLayersets = artLayerWr.GetParentLayerSets();
            LayerSet linesLayerSet = parentLayersets.Add();
            linesLayerSet.Name = "NewGroup";
            artLayerWr.CopyStyle();
            artLayerWr.OffStyle();
            var linesLayerSetWr = new  LayerSetWr(linesLayerSet);
            linesLayerSetWr.PasteStyle();
            linesLayerSetWr.OffStyle();

            List<ArtLayer> lineLayers = new List<ArtLayer>();

            var lines = artLayerWr.ArtLayer.TextItem.Contents.Split('\r');

            int lineCount = lines.Count();


            for (int i = 0; i < lineCount; i++)
            {
                ArtLayer copy = artLayerWr.ArtLayer.Duplicate(linesLayerSet, PsElementPlacement.psPlaceAtEnd);
                copy.TextItem.Contents = new string('\r', i) + lines[i];
                copy.Name = $"{artLayerWr.Name}_line{i + 0}";
                lineLayers.Add(copy);
            }
            artLayerWr.Visible = false;
            return linesLayerSet;
        }

    }
}
