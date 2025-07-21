using Photoshop;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    
    [Serializable]
    public class TextLeaf : LayerComposition
    {
        [XmlIgnore]
        public string Text = string.Empty;
        override public void Apply(DocumentWr doc)
        {
            ArtLayer layer = (GetLayerWr(doc) as ArtLayerWr).ArtLayer;
            layer.TextItem.Contents = Text?.Replace("\n", "\r");
        }
        public override bool IsMatching(DocumentWr doc)
        {
           return LayerDescriptor.Layer(LayerName,PsLayerKind.psTextLayer).DoesDocHas(doc);
        }
        protected override DtoConverter DtoConverter => new TextLeafDtoConverter();

        public TextLeaf():base() {  }
    }

    public class TextLeafDto:LayerCompostionDto
    {
        public string Text;
    }
    class TextLeafDtoConverter : LayerCompositionDtoConverter
    {
        public override void ApplyDto(object _obj, object _dto)
        {
            var obj = _obj as TextLeaf;
            var dto = (TextLeafDto)_dto;
            base.ApplyDto(_obj,_dto);
            obj.Text = dto.Text ?? string.Empty;
        }

        public override object GetDto(object _obj)
        {
            var obj = _obj as TextLeaf;
            var dto = new TextLeafDto();
            ExportDto(obj,dto);
            return dto;
        }
        protected override void ExportDto(object _obj, object _dto)
        {
            var dto = _dto as TextLeafDto;
            var obj = _obj as TextLeaf;
            base.ExportDto(_obj, _dto);
            dto.Text = obj.Text;
        }
    }
}

