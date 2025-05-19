using Photoshop;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {

        public static void TranslateV(this ArtLayer layer, Vector vector) =>
            layer.Translate(vector.X, vector.Y);
        public static void TranslateV(this LayerSet layer, Vector vector) =>
            layer.Translate(vector.X, vector.Y);
        public static Size GetBoundsSize(this ArtLayer layer)
        {
            return layer.GetBoundRect().Size;
        }
        public static Rect GetBoundRect(this ArtLayer layer) => _getBoundRect(layer);
        public static Rect GetBoundRect(this LayerSet layer) => _getBoundRect(layer);
        private static Rect _getBoundRect(dynamic layer)
        {
            return new Rect(new Point(layer.Bounds[0], layer.Bounds[1]), new Point(layer.Bounds[2], layer.Bounds[3]));
        }
        public static void OffStyle(this ArtLayer layer) => _offStyle(layer);
        public static void OffStyle(this LayerSet layer) => _offStyle(layer);
        private static void _offStyle(dynamic layer)
        {
            try
            {
                (layer as ArtLayer).MakeActive();
            }
            catch
            {
                (layer as LayerSet).MakeActive();
            }
            layer.Application.DoAction("offFx", "psdPH");
        }
        public static void OnStyle(this ArtLayer layer) => _onStyle(layer);
        public static void OnStyle(this LayerSet layer) => _onStyle(layer);
        private static void _onStyle(dynamic layer)
        {
            _makeActive(layer);
            layer.Application.DoAction("onFx", "psdPH");
        }

        public static Document GetDocument(this ArtLayer layer) => _getDocument(layer);
        public static Document GetDocument(this LayerSet layer) => _getDocument(layer);
        private static Document _getDocument(dynamic layer) => layer.Application.ActiveDocument;

        public static void MakeActive(this ArtLayer layer) => _makeActive(layer);
        public static void MakeActive(this LayerSet layer) => _makeActive(layer);
        private static void _makeActive(dynamic layer)
        {
            try
            {
                (layer as ArtLayer).GetDocument().ActiveLayer = layer;
            }
            catch
            {
                (layer as LayerSet).GetDocument().ActiveLayer = layer;
            }
        }
        public static void CopyStyle(this ArtLayer layer) => _copyStyle(layer);
        public static void CopyStyle(this LayerSet layer) => _copyStyle(layer);
        private static void _copyStyle(dynamic layer)
        {
            _makeActive(layer);
            layer.Application.DoAction("copyStyle", "psdPH");
        }
        public static void PasteStyle(this ArtLayer layer) => _pasteStyle(layer);
        public static void PasteStyle(this LayerSet layer) => _pasteStyle(layer);
        private static void _pasteStyle(dynamic layer)
        {
            _makeActive(layer);
            layer.Application.DoAction("pasteStyle", "psdPH");
        }
        public static LayerSets GetParentLayerSets(this ArtLayer artLayer) {
            dynamic parent = artLayer.Parent;
            LayerSets parentLayersets;
            try
            {
                var parent_doc = (parent as LayerSet);
                parentLayersets = parent_doc.LayerSets;
            }
            catch
            {
                var parent_group = (parent as Document);
                parentLayersets = parent_group.LayerSets;
            }
            return parentLayersets;
        }
        public static LayerSet GroupLayer(this ArtLayer artLayer)
        {
            LayerSets parentLayersets = artLayer.GetParentLayerSets();
            LayerSet newLayerSet = parentLayersets.Add();
            newLayerSet.Name = "NewGroup";
            artLayer.Move(newLayerSet, PsElementPlacement.psPlaceInside);
            return newLayerSet;
        }
        public static LayerSet SplitTextLayer(this ArtLayer artLayer)
        {
            LayerSets parentLayersets = artLayer.GetParentLayerSets();
            LayerSet linesLayerSet = parentLayersets.Add();
            linesLayerSet.Name = "NewGroup";
            artLayer.CopyStyle();
            artLayer.OffStyle();

            linesLayerSet.PasteStyle();
            linesLayerSet.OffStyle();

            List<ArtLayer> lineLayers = new List<ArtLayer>();

            var lines = artLayer.TextItem.Contents.Split('\r');

            int lineCount = lines.Count();


            for (int i = 0; i < lineCount; i++)
            {
                ArtLayer copy = artLayer.Duplicate(linesLayerSet, PsElementPlacement.psPlaceAtEnd) as ArtLayer;
                copy.TextItem.Contents = new string('\r', i) + lines[i];
                copy.Name = $"{artLayer.Name}_line{i + 0}";
                lineLayers.Add(copy);
            }
            artLayer.Visible = false;
            return linesLayerSet;
        }

    }
}
