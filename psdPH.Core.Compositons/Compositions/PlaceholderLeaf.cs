using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using psdPH.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    public class PlaceholderLeaf : LayerComposition
    {
        [XmlIgnore]
        public PrototypeBlob PrototypeBlob;
        public override string Name => LayerName;
        LayerBlob _replacement;
        [XmlIgnore]
        public LayerBlob Replacement
        {
            get => _replacement;
            set { _replacement = value; _replacement.LayerName = $"{PrototypeBlob.LayerName}_{LayerName}"; }
        }
        public PlaceholderLeaf() : base() {   }

        public void ReplaceWithFiller(DocumentWr docWr, LayerBlob blob)
        {
            LayerWr phLayerWr = LayerDescriptor.GetLayerWr(docWr);
            LayerWr originalLayerWr = PrototypeBlob.LayerDescriptor.GetLayerWr(docWr);
            originalLayerWr.Visible = true;
            ArtLayerWr newLayerWr = new ArtLayerWr(originalLayerWr.CloneSmartLayer());
            originalLayerWr.Visible = false;
            var prototypeAVector = PrototypeBlob.GetRelativeLayerAlightmentVector(docWr);
            var options = new AlignOptions(Alignment.Create("up", "left"), ConsiderFx.NoFx);
            var phAVector = newLayerWr.GetAlightmentVector(phLayerWr, options);

            newLayerWr.TranslateV(phAVector+prototypeAVector);
            phLayerWr.Opacity = 0;
            newLayerWr.Name = blob.LayerName;
        }

        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.IsInDoc(doc)
                && PrototypeBlob.IsMatching(doc);
        }

        public override void Apply(DocumentWr doc) { }
    }

}

