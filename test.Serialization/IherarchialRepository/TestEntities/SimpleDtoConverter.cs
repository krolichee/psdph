using psdPH.Serialization;
using System;

namespace test.Serialization
{
        class SimpleDtoConverter : DtoConverter
        {
            public override Type DtoType => typeof(SimpleDto);

            public override Type EntityType => typeof(SimpleEntity);

            protected override Dto CreateDto() => new SimpleDto();

            protected override object CreateEntity() => new SimpleEntity();

            protected override void UpdateDto(object obj, object dto)
            {
                var mapper = new SimpleDtoMapper();
                mapper.UpdateDto(obj, dto);
            }

            protected override void UpdateEntity(object obj, object dto)
            {
                var mapper = new SimpleDtoMapper();
                mapper.UpdateEntity(obj, dto);
            }
        }

    }

