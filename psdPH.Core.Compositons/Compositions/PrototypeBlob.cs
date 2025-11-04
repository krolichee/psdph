using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Windows;
using System.Xml.Serialization;
using psdPH.Logic.Serialization;
using psdPH.Photoshop;

namespace psdPH.Logic.Compositions
{
    public class PrototypeBlob:LayerBlob
    {
        public LayerDescriptor RelativeLayerDescriptor;
        [XmlIgnore]
        public string RelativeLayerName { get => RelativeLayerDescriptor.LayerName; set => RelativeLayerDescriptor = LayerDescriptor.Layer(value); }
        public override void Apply(DocumentWr doc) { }
        public PrototypeBlob() : base() {
            DtoConvertersRegistry.Register<PrototypeBlob>(new PrototypeBlobDtoConverter());
        }
        public Vector GetRelativeLayerAlightmentVector(DocumentWr docWr)
        {
            var options = new AlignOptions(Alignment.Create("up", "left"),ConsiderFx.NoFx);
            var layerWr = LayerDescriptor.GetLayerWr(docWr);
            var relLayerWr = RelativeLayerDescriptor.GetLayerWr(docWr);

            var bringToRelativeVector = relLayerWr.GetAlightmentVector(layerWr, options);
            return bringToRelativeVector;
        }
        public override bool IsMatching(DocumentWr doc)
        {
            return RelativeLayerDescriptor.DoesDocHas(doc)
                && base.IsMatching(doc);
        }
        public PlaceholderLeaf[] Placeholders => Siblings<PlaceholderLeaf>().Where(p=>p.PrototypeBlob==this).ToArray();
    }
    public class PrototypeBlobDto : LayerCompositionDto
    {
        public LayerDescriptor RelativeLayerDescriptor;
    }
    public class PrototypeBlobDtoConverter : LayerCompositionDtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
        {
            base.GetEntity(_obj, _dto);
            var obj = _obj as PrototypeBlob;
            var dto = _dto as PrototypeBlobDto;
            obj.RelativeLayerDescriptor = dto.RelativeLayerDescriptor;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            base.UpdateDto(_obj, _dto);
            var obj = _obj as PrototypeBlob;
            var dto = _dto as PrototypeBlobDto;
            dto.RelativeLayerDescriptor = obj.RelativeLayerDescriptor;
        }
        public override object GetDto(object _obj)
        {
            var dto = new PrototypeBlobDto();
            UpdateDto(_obj,dto);
            return dto;
        }
    }
}
