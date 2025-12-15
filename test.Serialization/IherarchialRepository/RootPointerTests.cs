using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{
    [TestClass]
    public class RootPointerTests
    {
        private Guid _testGuid;
        private RootPointer _rootPointer;
        private IdentityMap _identityMap;

        [TestInitialize]
        public void Setup()
        {
            _testGuid = Guid.NewGuid();
            _rootPointer = new RootPointer(_testGuid);
            _identityMap = new IdentityMap();
        }

        [TestMethod]
        public void Constructor_SetsRootGuid()
        {
            // Act
            var pointer = new RootPointer(_testGuid);

            // Assert - используем рефлексию для проверки приватного поля
            var field = typeof(RootPointer).GetProperty("RootGuid",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var value = (Guid)field.GetValue(pointer);

            Assert.AreEqual(_testGuid, value);
        }

        [TestMethod]
        public void GetRoot_ExistingGuidInMap_ReturnsObject()
        {
            // Arrange
            var expectedObject = new object();
            _identityMap.AddMapping(expectedObject, _testGuid);

            // Act
            var result = _rootPointer.GetRoot(_identityMap);

            // Assert
            Assert.AreSame(expectedObject, result);
        }

        [TestMethod]
        public void GetRoot_NonExistingGuidInMap_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _rootPointer.GetRoot(_identityMap));
        }

        [TestMethod]
        public void GetRoot_NullMap_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _rootPointer.GetRoot(null));
        }

        //TODO переписать под DtoScope
        //[TestMethod]
        //public void FindRootPointer_MapContainsRootPointer_ReturnsFirstRootPointer()
        //{
        //    // Arrange
        //    var otherObject = new object();
        //    _identityMap.AddMapping(otherObject, Guid.NewGuid());
        //    _identityMap.AddMapping(_rootPointer, Guid.NewGuid());

        //    // Act
        //    var result = RootPointer.FindRootPointer(_identityMap);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsInstanceOfType(result, typeof(RootPointer));
        //}

        //TODO переписать под DtoScope
        //[TestMethod]
        //public void FindRootPointer_MapDoesNotContainRootPointer_ThrowsInvalidOperationException()
        //{
        //    // Arrange
        //    _identityMap.AddMapping(new object(), Guid.NewGuid());

        //    // Act & Assert
        //    Assert.ThrowsException<InvalidOperationException>(() => RootPointer.FindRootPointer(_identityMap));
        //}

        //TODO переписать под DtoScope
        //[TestMethod]
        //public void FindRootPointer_EmptyMap_ThrowsInvalidOperationException()
        //{
        //    // Act & Assert
        //    Assert.ThrowsException<InvalidOperationException>(() => RootPointer.FindRootPointer(_identityMap));
        //}

        [TestMethod]
        public void FindRootPointer_NullMap_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => RootPointer.FindRootPointer(null));
        }
    }
}
