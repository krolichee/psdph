using Photoshop;
using psdPH.Photoshop;
using System;
using System.Linq;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Заглушка")]
    public class PlaceholderLeaf : LayerComposition, CoreComposition
    {
        [XmlIgnore]
        public PrototypeLeaf Prototype
        {
            get
            {
                return Siblings<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
            }
            set
            {
                PrototypeLayerName = value.LayerName;
            }
        }
        public string PrototypeLayerName;
        public override string ObjName => LayerName;

        public override Setup[] Setups => new Setup[0];
        Blob _replacement;
        [XmlIgnore]
        public Blob Replacement
        {
            get => _replacement;
            set { _replacement = value; _replacement.LayerName = $"{PrototypeLayerName}_{LayerName}"; }
        }
        public override void Apply(Document doc)
        {
            if (Replacement != null)
            {
                ReplaceWithFiller(doc, Replacement);
                Replacement.Apply(doc);
            }
        }

        public PlaceholderLeaf(string layername, string prototypeLayername)
        {
            LayerName = layername;
            PrototypeLayerName = prototypeLayername;
        }
        public PlaceholderLeaf() { }
        public override void RestoreParents(Composition parent = null)
        {
            base.RestoreParents(parent);
        }

        internal void ReplaceWithFiller(Document doc, Blob blob)
        {
            ArtLayer phLayer = doc.GetLayerByName(LayerName);
            ArtLayer originalLayer = doc.GetLayerByName(PrototypeLayerName);
            originalLayer.Visible = true;
            ArtLayerWr newLayerWr = new ArtLayerWr(doc.CloneSmartLayer(PrototypeLayerName));
            originalLayer.Visible = false;
            var prototypeAVector = Prototype.GetRelativeLayerAlightmentVector(doc);
            var options = new AlignOptions(Alignment.Create("up", "left"), LayerWr.ConsiderFx.NoFx);
            var phAVector = newLayerWr.GetAlightmentVector(new ArtLayerWr(phLayer), options);

            newLayerWr.TranslateV(phAVector+prototypeAVector);
            //ph_layer.Delete();
            phLayer.Opacity = 0;
            newLayerWr.Name = blob.LayerName;

            //Parent.addChild(blob);
            //Parent.removeChild(this);
        }

        public void CoreApply()
        {
            ((CoreComposition)Replacement).CoreApply();
        }

        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Layer(LayerName).DoesDocHas(doc)
                && Prototype.IsMatching(doc);
        }
    }

}

