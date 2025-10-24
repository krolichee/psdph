using Photoshop;
using System.Windows;
using System.Windows.Controls;
using Application = Photoshop.Application;

namespace psdPH.Photoshop
{

    public abstract partial class LayerWr
    {
        public class LayerStyle
        {
            LayerWr source;
            public bool Toggle
            {
                set
                {
                    if (value)
                        source.OnStyle();
                    else
                        source.OffStyle();
                }
            }
            public LayerStyle(LayerWr source)
            {
                this.source = source;
            }
            public void Paste(LayerWr dest)
            {
                if (dest.HasStyle() && source.HasStyle())
                {
                    source.CopyStyle();
                    dest.PasteStyle();
                }
            }
        }
        //LibMethods
        public abstract void Translate(double x, double y);
        public abstract void Resize(double w, double h);
        public abstract void Duplicate();
        public abstract void Duplicate(object dest, PsElementPlacement placement);
        public abstract void Move(object dest, PsElementPlacement placement);
        //LibProperties
        public abstract Application Application { get; }
        public abstract double[] Bounds { get; }
        public abstract double[] BoundsNoEffects { get; }
        public abstract dynamic Parent { get; }
        public abstract dynamic Name { get; set; }
        public abstract bool Visible { get; set; }
        public abstract double Opacity { get; set; }
        //CustomMethods
        public bool DoEffectsAffectBounds()
        {
            return GetRect(BoundsNoEffects) != GetRect(Bounds);
        }
        Rect GetRect(double[] bounds)
        {
            return new Rect(new Point(bounds[0], bounds[1]),
                new Point(bounds[2], bounds[3]));
        }
        public Rect GetNoFxBoundRect() => GetRect(BoundsNoEffects);
        public Size GetNoFxBoundsSize() => GetNoFxBoundRect().Size;
        public Rect GetBoundRect() => GetRect(Bounds);
        public Size GetBoundsSize() => GetBoundRect().Size;
        public bool HasStyle()
        {
            bool result = false;
            result = new KostylExecutor().tryAction(this.OffStyle);
            if (result)
            {
                var doc = this.GetActiveDocument();
                var lastIndex = doc.HistoryStates.Count - 1;
                doc.ActiveHistoryState = doc.HistoryStates[lastIndex];
            }
            return result;
        }
        public void OffStyle()
        {
            if (!HasStyle())
                return;
            MakeActive();
            Application.DoAction("offFx", "psdPH");
        }
        public void OnStyle()
        {
            if (!HasStyle())
                return;
            MakeActive();
            Application.DoAction("onFx", "psdPH");
        }
        public ArtLayer CloneSmartLayer()
        {
            this.Active = true;
            Application.DoAction("cloneSmartLayer", "psdPH");
            return GetActiveDocument().ActiveLayer;
        }
        public LayerStyle Style => new LayerStyle(this);
        public Document GetActiveDocument() => Application.ActiveDocument;
        public bool Active
        {
            get => IsActive();
            set
            {
                if (value)
                    MakeActive();
            }
        }
        protected abstract bool IsActive();
        protected abstract void MakeActive();
        void CopyStyle()
        {
            MakeActive();
            Application.DoAction("copyStyle", "psdPH");
        }
        void PasteStyle()
        {
            MakeActive();
            Application.DoAction("pasteStyle", "psdPH");
        }
        public void TranslateV(Vector vector)
        {
            Translate(vector.X, vector.Y);
        }
        protected LayerSets GetParentLayerSets()
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
            return newLayerSet.Wrapper();
        }
        delegate Rect BoundFunction(LayerWr layerWr);
        public Rect GetBoundRect(ConsiderFx considerFx) =>
            getBoundFunc(considerFx)(this);
        static BoundFunction getBoundFunc(ConsiderFx considerFx)
        {
            BoundFunction func;
            if (considerFx == ConsiderFx.WithFx)
                func = (LayerWr l) => l.GetBoundRect();
            else
                func = (LayerWr l) => l.GetNoFxBoundRect();
            return func;
        }

        public void FixLayerName()
        {
            var layername = Name;
            Name = "_";
            Name = layername;
        }

    }
}
