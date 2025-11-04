using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using psdPH.Serialization.Serialization;
using psdPH.Serialization;

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
    public class PlaceholderDto : LayerCompositionDto
    {
        public Guid PrototypeGuid;
    }
    class PlaceholderDtoConverter : DtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
        {
            var obj = _obj as PlaceholderLeaf;
            var dto = _dto as PlaceholderDto;
            base.GetEntity(_obj, _dto);
            GuidScope.Current.GuidsLoaded += () => 
            obj.PrototypeBlob = GuidScope.Current.GetByGuid(dto.PrototypeGuid) as PrototypeBlob;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var obj = _obj as PlaceholderLeaf;
            var dto = _dto as PlaceholderDto;
            new LayerCompositionDtoConverter().UpdateDto(_obj, _dto);
            dto.PrototypeGuid = obj.PrototypeBlob.Guid;
        }

        public override object GetDto(object _obj)
        {
            var dto = new PlaceholderDto();
            UpdateDto(_obj, dto);
            return dto;
        }
    }
    class PlaceholderDtoMapper : DtoMapper<PlaceholderLeaf, PlaceholderDto>
    {
        public override void UpdateDto(PlaceholderLeaf entity, PlaceholderDto dto)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEntity(PlaceholderLeaf entity, PlaceholderDto dto)
        {
            throw new NotImplementedException();
        }
    }

}

