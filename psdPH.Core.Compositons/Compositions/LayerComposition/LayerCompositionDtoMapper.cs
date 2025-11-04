using psdPH.Logic.Compositions;
using psdPH.Serialization.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Core.Compositons.Compositions.LayerComosition
{
    class LayerCompositionDtoMapper : DtoMapper<LayerComposition, LayerCompositionDto>
    {
        protected override void MapEntityToDto(LayerComposition entity, LayerCompositionDto dto)
        {
            entity.LayerDescriptor = dto.LayerDescriptor;
        }
        protected override void MapDtoToEntity(LayerComposition entity, LayerCompositionDto dto)
        {
            dto.LayerDescriptor = entity.LayerDescriptor;
        }
    }
}
