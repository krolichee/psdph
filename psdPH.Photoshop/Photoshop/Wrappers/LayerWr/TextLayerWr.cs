using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Photoshop
{
    public class TextLayerWr : ArtLayerWr
    {
        public TextLayerWr(ArtLayer layer) : base(layer)
        {
            if (layer.Kind != PsLayerKind.psTextLayer)
                throw new ArgumentException();
        }
        public LayerSetWr SplitTextLayer()
        {
            LayerSets parentLayersets = GetParentLayerSets();
            LayerSet linesLayerSet = parentLayersets.Add();
            linesLayerSet.Name = $"{Name}_Split";
            var linesLayerSetWr = new LayerSetWr(linesLayerSet);
            List<ArtLayer> lineLayers = new List<ArtLayer>();

            var lines = ArtLayer.TextItem.Contents.Split('\r');

            int lineCount = lines.Count();
            for (int i = 0; i < lineCount; i++)
            {
                ArtLayer copy = ArtLayer.Duplicate(linesLayerSet, PsElementPlacement.psPlaceAtEnd);
                copy.TextItem.Contents = new string('\r', i) + lines[i];
                copy.Name = $"{Name}_line{i + 0}";
                lineLayers.Add(copy);
            }
            Visible = false;
            return linesLayerSet.Wrapper();
        }
    }
}
