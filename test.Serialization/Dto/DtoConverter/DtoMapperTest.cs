using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{
	class TestDto : Dto
	{
        public Guid Guid { get; }
        public int[] ints;
	}
    class NotDto { }
    class TestDtoMapper : DtoMapper<List<int>, TestDto>
    {
        protected override void MapDtoToEntity(List<int> entity, TestDto dto)
        {
            entity.Clear();
            entity.AddRange(dto.ints);
        }

        protected override void MapEntityToDto(List<int> entity, TestDto dto)
        {
            dto.ints = entity.ToArray();
        }
    }
    [TestClass]
	public class DtoMapperTest
	{
		[TestMethod]
		public void ValidationTest()
		{
            var entity = new List<int>();
            var wrongEntity = new List<bool>();
            var dto = new TestDto();
            var wrongDto = new NotDto();
            var mapper = new TestDtoMapper();
            Assert.ThrowsException<ArgumentException>((() => mapper.UpdateEntity(wrongEntity, dto)));
            Assert.ThrowsException<ArgumentException>((() => mapper.UpdateDto(wrongEntity, dto)));
            Assert.ThrowsException<ArgumentException>((() => mapper.UpdateEntity(entity, wrongDto)));
            Assert.ThrowsException<ArgumentException>((() => mapper.UpdateDto(entity, wrongDto)));
		}
        [TestMethod]
        public void UpdateDtoTest()
        {
            var entity = new List<int>() { 1, 2, 3 };
            var dto = new TestDto();
            var mapper = new TestDtoMapper();
            mapper.UpdateDto(entity, dto);
            Console.WriteLine(dto.ints.Count());
            Assert.IsTrue(entity.ToArray().Intersect(dto.ints).Count()==entity.Count());
        }
        [TestMethod]
        public void UpdateEntityTest()
        {
            var entity = new List<int>();
            var dto = new TestDto() { ints = new int[]{ 1, 2, 3 } };
            var mapper = new TestDtoMapper();
            mapper.UpdateEntity(entity, dto);
            Console.WriteLine(dto.ints.Count());
            Assert.IsTrue(entity.ToArray().Intersect(dto.ints).Count() == entity.Count());
        }
    }
}
