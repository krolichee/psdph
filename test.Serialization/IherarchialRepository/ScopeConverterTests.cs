using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{
    [TestClass]
    public class ScopeConverterTests
    {
        private class TestDto : Dto { }
        private class TestEntity { }

        public class TestDtoConverter : DtoConverter
        {
            public override Type DtoType => typeof(TestDto);
            public override Type EntityType => typeof(TestEntity);

            private readonly Guid _fixedGuid;
            private readonly UnknownEntityReference[] _pendingReferences;

            public TestDtoConverter(Guid fixedGuid = default, UnknownEntityReference[] pendingReferences = null)
            {
                _fixedGuid = fixedGuid != default ? fixedGuid : Guid.NewGuid();
                _pendingReferences = pendingReferences ?? Array.Empty<UnknownEntityReference>();
            }

            public TestDtoConverter()
            {
                _pendingReferences = Array.Empty<UnknownEntityReference>();
            }

            protected override object CreateEntity() => new TestEntity();
            protected override Dto CreateDto() => new TestDto();
            protected override void UpdateDto(object obj, object dto) { }
            protected override void UpdateEntity(object obj, object dto) { }
            public Guid GetDtoGuid(object dto) => _fixedGuid;
            //protected override UnknownEntityReference[] GetUnknownEntityReferences(object obj, Dto dto) => _pendingReferences;
            protected override UnknownEntityReference[] GetUnknownEntityReferences(object obj, Dto dto) => _pendingReferences;
        }

        [TestInitialize]
        public void Setup()
        {
            // Очищаем registry перед каждым тестом
            ClearRegistry();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Очищаем registry после каждого теста
            ClearRegistry();
        }

        private void ClearRegistry()
        {
            // Используем рефлексию для очистки registry
            var field = typeof(DtoConvertersRegistry).GetField("converters",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (field != null)
            {
                var converters = (HashSet<DtoConverter>)field.GetValue(null);
                converters.Clear();
            }
        }

        [TestMethod]
        public void ConvertDtoScope_WithSingleDto_ReturnsContextWithIdentityAndNoReferences()
        {
            // Arrange
            var testGuid = Guid.NewGuid();
            var converter = new TestDtoConverter(testGuid);
            DtoConvertersRegistry.Register(converter);

            var dtoScope = new DtoScope();
            dtoScope.Scope.Add(new TestDto());
            
            // Act
            var result = ScopeConverter.ConvertDtoScope(dtoScope);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.IdentityMap);
            Assert.IsNotNull(result.PendingReferences);
            Assert.AreEqual(1, result.IdentityMap.Count);
            Assert.AreEqual(0, result.PendingReferences.Length);
            Assert.IsTrue(result.IdentityMap.TryGetObject(testGuid, out var entity));
            Assert.IsInstanceOfType(entity, typeof(TestEntity));
        }


        [TestMethod]
        public void ConvertDtoScope_WithPendingReferences_CollectsAllReferences()
        {
            // Arrange
            var targetGuid = Guid.NewGuid();
            var pendingRefs = new[]
            {
            new UnknownEntityReference { TargetEntityGuid = targetGuid, ReferenceSetter = obj => { } },
            new UnknownEntityReference { TargetEntityGuid = targetGuid, ReferenceSetter = obj => { } }
        };

            var converter = new TestDtoConverter(Guid.NewGuid(), pendingRefs);
            DtoConvertersRegistry.Register(converter);

            var dtoScope = new DtoScope();
            dtoScope.Scope.Add(new TestDto());

            // Act
            var result = ScopeConverter.ConvertDtoScope(dtoScope);

            // Assert
            Assert.AreEqual(2, result.PendingReferences.Length);
            Assert.AreEqual(targetGuid, result.PendingReferences[0].TargetEntityGuid);
            Assert.AreEqual(targetGuid, result.PendingReferences[1].TargetEntityGuid);
        }

        [TestMethod]
        public void ConvertDtoScope_EmptyScope_ReturnsEmptyContext()
        {
            // Arrange
            DtoConverterRegistrator.InitializeRegistry();
            var dtoScope = new DtoScope();

            // Act
            var result = ScopeConverter.ConvertDtoScope(dtoScope);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.IdentityMap.Count);
            Assert.AreEqual(0, result.PendingReferences.Length);
        }

        [TestMethod]
        public void ConvertDtoScope_NullScope_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => ScopeConverter.ConvertDtoScope(null));
        }

        [TestMethod]
        public void ConvertDtoScope_NoConverterRegisteredForDto_ThrowsInvalidOperationException()
        {
            // Arrange
            // Не регистрируем никаких конвертеров
            var dtoScope = new DtoScope();
            dtoScope.Scope.Add(new TestDto());

            // Act & Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(
                () => ScopeConverter.ConvertDtoScope(dtoScope));

            StringAssert.Contains(exception.Message, "No converter found");
        }

        //TODO тест для конверсии, где встретились одинаковые DTO, конверсия которых даёт один guid
    }
}
