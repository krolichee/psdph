using Photoshop;
using psdPH.Photoshop;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    public abstract class LayerComposition : Composition
    {
        [XmlIgnore]
        public string LayerName { get => LayerDescriptor.LayerName; set => LayerDescriptor = LayerDescriptor.Layer(value); }
        [XmlIgnore]
        public LayerDescriptor LayerDescriptor;
        public override string ObjName => LayerName;
        public ArtLayerWr ArtLayerWr(Document doc)
        {
            return doc.GetLayerByName(LayerName).Wrapper();
        }
        public LayerComposition(string layername) { LayerDescriptor = LayerDescriptor.Layer(layername); }
        public LayerComposition() {
        }
        protected LayerWr getLayerWr(Document doc, string layerName) => doc.GetLayerWrByName(layerName);
    }
    public class LayerCompostionDto:Dto
    {
        public LayerDescriptor LayerDescriptor;
    }
    public abstract class LayerCompositionDtoConverter : DtoConverter
    {
        public override void ApplyDto(object _obj, object _dto)
        {
            var obj = _obj as LayerComposition;
            var dto = _dto as LayerCompostionDto;
            obj.LayerDescriptor = dto.LayerDescriptor;
        }

        public override object GetDto(object _obj)
        {
            var dto = new LayerCompostionDto();
            ExportDto(_obj,dto);
            return dto;
        }
        public override void ExportDto(object _obj, object _dto)
        {
            var obj = _obj as LayerComposition;
            var dto = _dto as LayerCompostionDto;
            base.ExportDto(_obj, _dto);
            dto.LayerDescriptor = obj.LayerDescriptor;
        }
    }

}

