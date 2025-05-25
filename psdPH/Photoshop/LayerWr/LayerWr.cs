using Photoshop;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static psdPH.Logic.PhotoshopDocumentExtension;
using Application = Photoshop.Application;

namespace psdPH.Photoshop
{
    public abstract class LayerWr
    {
        //LibMethods
        public abstract void Translate(double x, double y);
        public abstract void Resize(double w, double h);
        public abstract void Duplicate();
        public abstract void Duplicate(object dest, PsElementPlacement placement);
        public abstract void Move(object dest, PsElementPlacement placement);
        //LibProperties
        public abstract Application Application { get; }
        public abstract double[] Bounds { get; }
        public abstract dynamic Parent { get; }
        public abstract dynamic Name { get; set; }
        public abstract bool Visible { get; set; }
        public abstract double Opacity { get; set; }
        //CustomMethods
        public Rect GetBoundRect()
        {
            double[] bounds = Bounds;
            return new Rect(new Point(bounds[0], bounds[1]),
                new Point(bounds[2], bounds[3]));
        }
        public Size GetBoundsSize()=>GetBoundRect().Size;
        public void OffStyle()
        {
            MakeActive();
            Application.DoAction("offFx", "psdPH");
        }
        public void OnStyle()
        {
            MakeActive();
            Application.DoAction("onFx", "psdPH");
        }
        public Document GetActiveDocument() => Application.ActiveDocument;
        public abstract void MakeActive();
        public void CopyStyle()
        {
            MakeActive();
            Application.DoAction("copyStyle", "psdPH");
        }
        public void PasteStyle()
        {
            MakeActive();
            Application.DoAction("pasteStyle", "psdPH");
        }
        public void TranslateV(Vector vector)
        {
            Translate(vector.X, vector.Y);
        }
        public LayerSets GetParentLayerSets()
        {
            dynamic parent = Parent;
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
        public LayerSetWr GroupLayer()
        {
            LayerSets parentLayersets = GetParentLayerSets();
            LayerSet newLayerSet = parentLayersets.Add();
            newLayerSet.Name = "NewGroup";
            Move(newLayerSet, PsElementPlacement.psPlaceInside);
            return newLayerSet.Wrapper() ;
        }
        
    }
    public partial class ArtLayerWr : LayerWr
    {
        private ArtLayer _layer;
        public ArtLayer ArtLayer { get => _layer; }
        public ArtLayerWr(ArtLayer layer) =>
            _layer = layer;
        
    }
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
            linesLayerSet.Name = "NewGroup";
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
    public partial class LayerSetWr : LayerWr
    {
        private LayerSet _layer;
        public LayerSet LayerSet { get => _layer; }
        public ArtLayers ArtLayers => _layer.ArtLayers;
        public LayerSetWr(LayerSet layer) =>
            _layer = layer;
    }
    public static class WrapperExtension
    {
        public static ArtLayerWr Wrapper(this ArtLayer layer) => new ArtLayerWr(layer);
        public static LayerSetWr Wrapper(this LayerSet layer) => new LayerSetWr(layer);
        public static TextLayerWr TextWrapper(this ArtLayer layer) => new TextLayerWr(layer);
    }
}
