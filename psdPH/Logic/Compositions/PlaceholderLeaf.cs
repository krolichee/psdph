using Photoshop;
using psdPH.Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Linq;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Заглушка")]
    public class PlaceholderLeaf : LayerComposition
    {


        [XmlIgnore]
        public PrototypeBlob PrototypeBlob;
        public override string ObjName => LayerName;
        LayerBlob _replacement;
        [XmlIgnore]
        public LayerBlob Replacement
        {
            get => _replacement;
            set { _replacement = value; _replacement.LayerName = $"{PrototypeBlob.LayerName}_{LayerName}"; }
        }
        public override void Apply(Document doc)
        {
            if (Replacement != null)
            {
                ReplaceWithFiller(doc, Replacement);
                Replacement.Apply(doc);
            }
        }
        public PlaceholderLeaf() { }
        public override void RestoreParents(Composition parent = null)
        {
            base.RestoreParents(parent);
        }

        internal void ReplaceWithFiller(Document doc, LayerBlob blob)
        {
            LayerWr phLayerWr = LayerDescriptor.GetFromDoc(doc);
            LayerWr originalLayerWr = PrototypeBlob.LayerDescriptor.GetFromDoc(doc);
            originalLayerWr.Visible = true;
            ArtLayerWr newLayerWr = new ArtLayerWr(doc.CloneSmartLayer(originalLayerWr.Name as string));
            originalLayerWr.Visible = false;
            var prototypeAVector = PrototypeBlob.GetRelativeLayerAlightmentVector(doc);
            var options = new AlignOptions(Alignment.Create("up", "left"), LayerWr.ConsiderFx.NoFx);
            var phAVector = newLayerWr.GetAlightmentVector(phLayerWr, options);

            newLayerWr.TranslateV(phAVector+prototypeAVector);
            //ph_layer.Delete();
            phLayerWr.Opacity = 0;
            newLayerWr.Name = blob.LayerName;

            //Parent.addChild(blob);
            //Parent.removeChild(this);
        }

        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.DoesDocHas(doc)
                && PrototypeBlob.IsMatching(doc);
        }
    }
    public class PlaceholderDto : LayerCompostionDto
    {
        public Guid PrototypeGuid;
    }
    class PlaceholderDtoConverter : LayerCompositionDtoConverter
    {
        public override void ApplyDto(object _obj, object _dto)
        {
            throw new NotImplementedException();
        }

        public override object GetDto(object _obj)
        {
            throw new NotImplementedException();
        }
    }

}

