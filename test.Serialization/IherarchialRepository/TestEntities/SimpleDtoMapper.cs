using psdPH.Serialization;

namespace test.Serialization
{
        class SimpleDtoMapper : DtoMapper<SimpleEntity, SimpleDto>
        {
            protected override void MapDtoToEntity(SimpleEntity entity, SimpleDto dto)
            {
                entity.a = dto.a;
            }

            protected override void MapEntityToDto(SimpleEntity entity, SimpleDto dto)
            {
                dto.a = entity.a;
            }
        }

    }

