using Photoshop;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    public class TextLeaf : LayerComposition
    {
        string Text = string.Empty;
        override public void Apply(DocumentWr doc)
        {
            ArtLayer layer = (GetLayerWr(doc) as ArtLayerWr).ArtLayer;
            layer.TextItem.Contents = Text?.Replace("\n", "\r");
        }
        public override bool IsMatching(DocumentWr doc)
        {
           return LayerDescriptor.Layer(LayerName,PsLayerKind.psTextLayer).IsInDoc(doc);
        }

        public TextLeaf():base() {  }
    }

    public class TextLeafDto:LayerCompositionDto
    {
        //TODO Глобально, композиция не должна хранить конкретные значения,
        //она только должна иметь методы для установки значений при рендере
        
    }
    class TextLeafDtoConverter : LayerCompositionDtoConverter
    {
        override 
        public override void GetEntity(object _obj, object _dto)
        {
            var obj = _obj as TextLeaf;
            var dto = (TextLeafDto)_dto;
            base.GetEntity(_obj,_dto);
            obj.Text = dto.Text ?? string.Empty;
        }

        public override object GetDto(object _obj)
        {
            var obj = _obj as TextLeaf;
            var dto = new TextLeafDto();
            UpdateDto(obj,dto);
            return dto;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var dto = _dto as TextLeafDto;
            var obj = _obj as TextLeaf;
            base.UpdateDto(_obj, _dto);
            dto.Text = obj.Text;
        }
    }
}

