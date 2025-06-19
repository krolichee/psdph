using Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    
    [Serializable]
    [UIName("Текст")]
    public class TextLeaf : LayerComposition
    {
        [XmlIgnore]
        public string Text = string.Empty;
        override public void Apply(Document doc)
        {
            ArtLayer layer = ArtLayerWr(doc).ArtLayer;
            layer.TextItem.Contents = Text?.Replace("\n", "\r");
        }
        public override bool IsMatching(Document doc)
        {
           return LayerDescriptor.Layer(LayerName,PsLayerKind.psTextLayer).DoesDocHas(doc);
        }
        public TextLeaf()
        {
            DtoConvertersRegistry.Register<TextLeaf>(new TextLeafDtoConverter());
        }
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
        public override void ExportDto(object _obj, object _dto)
        {
            var dto = _dto as TextLeafDto;
            var obj = _obj as TextLeaf;
            base.ExportDto(_obj, _dto);
            dto.Text = obj.Text;
        }
    }
}

