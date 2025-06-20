using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Windows;
using System.Xml.Serialization;
using psdPH.Logic.Serialization;

namespace psdPH.Logic.Compositions
{
    public class PrototypeBlob:LayerBlob
    {
        public LayerDescriptor RelativeLayerDescriptor;
        [XmlIgnore]
        public string RelativeLayerName { get => RelativeLayerDescriptor.LayerName; set => RelativeLayerDescriptor = LayerDescriptor.Layer(value); }
        public override void Apply(Document doc) { }
        public PrototypeBlob() : base() {
            DtoConvertersRegistry.Register<PrototypeBlob>(new PrototypeBlobDtoConverter());
        }
        public Vector GetRelativeLayerAlightmentVector(Document doc)
        {
            var options = new AlignOptions(Alignment.Create("up", "left"), Photoshop.LayerWr.ConsiderFx.NoFx);
            var layerWr = LayerDescriptor.GetFromDoc(doc);
            var relLayerWr = RelativeLayerDescriptor.GetFromDoc(doc);

            var bringToRelativeVector = relLayerWr.GetAlightmentVector(layerWr, options);
            return bringToRelativeVector;
        }
        public override bool IsMatching(Document doc)
        {
            return RelativeLayerDescriptor.DoesDocHas(doc)
                && base.IsMatching(doc);
        }
        public PlaceholderLeaf[] Placeholders => Siblings<PlaceholderLeaf>().Where(p=>p.PrototypeBlob==this).ToArray();
    }
    public class PrototypeBlobDto : LayerCompostionDto
    {
        public LayerDescriptor RelativeLayerDescriptor;
    }
    public class PrototypeBlobDtoConverter : LayerCompositionDtoConverter
    {
        public override void ApplyDto(object _obj, object _dto)
        {
            base.ApplyDto(_obj, _dto);
            var obj = _obj as PrototypeBlob;
            var dto = _dto as PrototypeBlobDto;
            obj.RelativeLayerDescriptor = dto.RelativeLayerDescriptor;
        }
        public override void ExportDto(object _obj, object _dto)
        {
            base.ExportDto(_obj, _dto);
            var obj = _obj as PrototypeBlob;
            var dto = _dto as PrototypeBlobDto;
            dto.RelativeLayerDescriptor = obj.RelativeLayerDescriptor;
        }
        public override object GetDto(object _obj)
        {
            var dto = new PrototypeBlobDto();
            ExportDto(_obj,dto);
            return dto;
        }
    }
}
