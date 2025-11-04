using psdPH.Logic.Serialization;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;

namespace psdPH.Logic.Compositions
{
    [Serializable]
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
        public override void Apply(DocumentWr doc)
        {
            if (Replacement != null)
            {
                ReplaceWithFiller(doc, Replacement);
                Replacement.Apply(doc);
            }
        }
        protected override DtoConverter DtoConverter => new PlaceholderDtoConverter();
        public PlaceholderLeaf() : base() {   }
        //Зачем это переопределять?
        public override void RestoreParents(Composition parent = null)
        {
            base.RestoreParents(parent);
        }

        internal void ReplaceWithFiller(DocumentWr docWr, LayerBlob blob)
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
            //ph_layer.Delete();
            phLayerWr.Opacity = 0;
            newLayerWr.Name = blob.LayerName;

            //Parent.addChild(blob);
            //Parent.removeChild(this);
        }

        public override bool IsMatching(DocumentWr doc)
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
            var obj = _obj as PlaceholderLeaf;
            var dto = _dto as PlaceholderDto;
            base.ApplyDto(_obj, _dto);
            GuidScope.Current.GuidsLoaded += () => 
            obj.PrototypeBlob = GuidScope.Current.GetByGuid(dto.PrototypeGuid) as PrototypeBlob;
        }
        protected override void ExportDto(object _obj, object _dto)
        {
            var obj = _obj as PlaceholderLeaf;
            var dto = _dto as PlaceholderDto;
            base.ExportDto(_obj, _dto);
            dto.PrototypeGuid = obj.PrototypeBlob.Guid;
        }

        public override object GetDto(object _obj)
        {
            var dto = new PlaceholderDto();
            ExportDto(_obj, dto);
            return dto;
        }
    }

}

