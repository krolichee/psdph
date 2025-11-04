using psdPH.Core.Compositons.Compositions.LayerComosition;
using psdPH.Serialization;

namespace psdPH.Logic.Compositions
{
    public class LayerCompositionDtoConverter : DtoConverter
    {
        protected override object CreateEntity()
        {
            return new LayerCompositionDto();
        }
        protected override Dto CreateDto()
        {
            return new LayerCompositionDto();
        }
        protected override void UpdateEntity(object _obj, object _dto)
        {
            new LayerCompositionDtoMapper().UpdateEntity(_obj, _dto);
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            new LayerCompositionDtoMapper().UpdateDto(_obj, _dto);
        }
    }

}

