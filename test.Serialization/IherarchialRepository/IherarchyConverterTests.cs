using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Serialization.IherarchialRepository
{
    [TestClass]
    public class IherarchyConverterTests
    {
        class SimpleEntity
        {
           public int a;

            public SimpleEntity()
            {
            }

            public SimpleEntity(int a)
            {
                this.a = a;
            }
        }
        class SimpleDto : Dto
        {
            public int a;
        }
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
        class TestEntity
        {
            public object Ref1;
            public object Ref2;
        }
        class TestDto : Dto
        {
            public Guid Ref1;
            public Guid Ref2;
        }
        class TestDtoConverter : DtoConverter
        {
            public override Type DtoType => typeof(TestDto);

            public override Type EntityType => typeof(TestEntity);

            protected override Dto CreateDto()
            {
                return new TestDto();
            }

            protected override object CreateEntity()
            {
                return new TestEntity();
            }

            protected override void UpdateDto(object obj, object dto)
            {
            }

            protected override void UpdateEntity(object obj, object dto)
            {
            }
            protected override UnknownGuidReference[] GetUnknownGuidReferences(object obj, Dto dto)
            {
                var _obj = obj as TestEntity;
                var _dto = dto as TestDto;
                return new UnknownGuidReference[]
                {
                    new UnknownGuidReference(){TargetEntity = _obj.Ref1,ReferenceSetter=(id)=>_dto.Ref1=id},
                    new UnknownGuidReference(){TargetEntity = _obj.Ref2,ReferenceSetter=(id)=>_dto.Ref2=id}
                };
            }
        }

        [TestMethod]
        public void SerializationTest()
        {
            DtoConverterRegistrator.InitializeRegistry();
            var entity = new TestEntity();
            entity.Ref1 = new SimpleEntity(1);
            entity.Ref2 = new SimpleEntity(2);
            var dtoScope = new IherarchyConverter().GetRelatedDtoScopeFromRootEntity(entity);
            Assert.IsTrue(dtoScope.Scope.Any(o => (o is SimpleDto) ? (o as SimpleDto).a == 1 : false));
            Assert.IsTrue(dtoScope.Scope.Any(o => (o is SimpleDto) ? (o as SimpleDto).a == 2 : false));
        }
    }
}
