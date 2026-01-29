using psdPH.Serialization;
using System;

namespace test.Serialization
{
        class RefDtoConverter : DtoConverter
        {
            public override Type DtoType => typeof(RefDto);

            public override Type EntityType => typeof(RefEntity);

            protected override Dto CreateDto()
            {
                return new RefDto();
            }

            protected override object CreateEntity()
            {
                return new RefEntity();
            }

            protected override void UpdateDto(object obj, object dto)
            {
            }

            protected override void UpdateEntity(object obj, object dto)
            {
            }
            protected override PendingGuidReference[] GetUnknownGuidReferences(object obj, Dto dto)
            {
                var _obj = obj as RefEntity;
                var _dto = dto as RefDto;
                return new PendingGuidReference[]
                {
                    new PendingGuidReference(){TargetEntity = _obj.Ref1,ReferenceSetter=(id)=>_dto.Ref1=id},
                    new PendingGuidReference(){TargetEntity = _obj.Ref2,ReferenceSetter=(id)=>_dto.Ref2=id}
                };
            }
        protected override PendingEntityReference[] GetUnknownEntityReferences(object obj, Dto dto)
        {
            var _obj = obj as RefEntity;
            var _dto = dto as RefDto;
            return new PendingEntityReference[]
            {
                    new PendingEntityReference(){TargetEntityGuid = _dto.Ref1,ReferenceSetter=(ent)=>_obj.Ref1=ent},
                    new PendingEntityReference(){TargetEntityGuid = _dto.Ref2,ReferenceSetter=(ent)=>_obj.Ref2=ent}
            };
        }
    }

    }

