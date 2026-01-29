using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{
    [TestClass]
    public class IdentityMapTests
    {
        private IdentityMap _map;
        private object _testObject;
        private Guid _testGuid;

        [TestInitialize]
        public void Setup()
        {
            _map = new IdentityMap();
            _testObject = new object();
            _testGuid = Guid.NewGuid();
        }

        [TestMethod]
        public void GetOrCreateId_NewObject_AddsAndReturnsNewGuid()
        {
            // Act
            var result = _map.GetOrCreateId(_testObject);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result);
            Assert.IsTrue(_map.TryGetObject(result, out var obj));
            Assert.AreSame(_testObject, obj);
        }

        [TestMethod]
        public void GetOrCreateId_ExistingObject_ReturnsSameGuid()
        {
            // Arrange
            var firstId = _map.GetOrCreateId(_testObject);

            // Act
            var secondId = _map.GetOrCreateId(_testObject);

            // Assert
            Assert.AreEqual(firstId, secondId);
        }

        [TestMethod]
        public void GetOrCreateId_NullObject_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _map.GetOrCreateId(null));
        }

        [TestMethod]
        public void TryGetId_ExistingObject_ReturnsTrueAndGuid()
        {
            // Arrange
            var expectedId = _map.GetOrCreateId(_testObject);

            // Act
            var result = _map.TryGetId(_testObject, out var actualId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedId, actualId);
        }

        [TestMethod]
        public void TryGetId_NonExistingObject_ReturnsFalse()
        {
            // Act
            var result = _map.TryGetId(new object(), out var id);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, id);
        }

        [TestMethod]
        public void TryGetId_NullObject_ReturnsFalse()
        {
            // Act
            var result = _map.TryGetId(null, out var id);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, id);
        }

        [TestMethod]
        public void TryGetObject_ExistingGuid_ReturnsTrueAndObject()
        {
            // Arrange
            var id = _map.GetOrCreateId(_testObject);

            // Act
            var result = _map.TryGetObject(id, out var obj);

            // Assert
            Assert.IsTrue(result);
            Assert.AreSame(_testObject, obj);
        }

        [TestMethod]
        public void TryGetObject_NonExistingGuid_ReturnsFalse()
        {
            // Act
            var result = _map.TryGetObject(Guid.NewGuid(), out var obj);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void GetId_ExistingObject_ReturnsGuid()
        {
            // Arrange
            var expectedId = _map.GetOrCreateId(_testObject);

            // Act
            var actualId = _map.GetId(_testObject);

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [TestMethod]
        public void GetId_NonExistingObject_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _map.GetId(new object()));
        }

        [TestMethod]
        public void GetObject_ExistingGuid_ReturnsObject()
        {
            // Arrange
            var id = _map.GetOrCreateId(_testObject);

            // Act
            var result = _map.GetObject(id);

            // Assert
            Assert.AreSame(_testObject, result);
        }

        [TestMethod]
        public void GetObject_NonExistingGuid_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _map.GetObject(Guid.NewGuid()));
        }

        [TestMethod]
        public void AddMapping_NewMapping_AddsSuccessfully()
        {
            // Act
            _map.AddMapping(_testObject, _testGuid);

            // Assert
            Assert.IsTrue(_map.TryGetId(_testObject, out var id));
            Assert.AreEqual(_testGuid, id);
            Assert.IsTrue(_map.TryGetObject(_testGuid, out var obj));
            Assert.AreSame(_testObject, obj);
        }

        [TestMethod]
        public void AddMapping_DuplicateGuid_ThrowsInvalidOperationException()
        {
            // Arrange
            var otherObject = new object();
            _map.AddMapping(otherObject, _testGuid);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _map.AddMapping(_testObject, _testGuid));
        }

        [TestMethod]
        public void AddMapping_NullObject_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _map.AddMapping(null, _testGuid));
        }

        [TestMethod]
        public void Clear_RemovesAllMappings()
        {
            // Arrange
            _map.GetOrCreateId(_testObject);

            // Act
            _map.Clear();

            // Assert
            Assert.AreEqual(0, _map.Count);
            Assert.IsFalse(_map.TryGetId(_testObject, out _));
        }

        [TestMethod]
        public void Count_ReturnsCorrectNumberOfMappings()
        {
            // Arrange
            var obj1 = new object();
            var obj2 = new object();

            // Act
            _map.GetOrCreateId(obj1);
            _map.GetOrCreateId(obj2);

            // Assert
            Assert.AreEqual(2, _map.Count);
        }

        [TestMethod]
        public void Constructor_WithIdentities_AddsAllMappings()
        {
            // Arrange
            var identities = new[]
            {
            new Identity(Guid.NewGuid(), new object()),
            new Identity(Guid.NewGuid(), new object())
        };

            // Act
            var map = new IdentityMap(identities);

            // Assert
            Assert.AreEqual(identities.Length, map.Count);
            foreach (var identity in identities)
            {
                Assert.IsTrue(map.TryGetObject(identity.Guid, out var obj));
                Assert.AreSame(identity.Entity, obj);
            }
        }

        [TestMethod]
        public void Objects_ReturnsAllObjects()
        {
            // Arrange
            var obj1 = new object();
            var obj2 = new object();
            _map.GetOrCreateId(obj1);
            _map.GetOrCreateId(obj2);

            // Act
            var objects = _map.Objects;

            // Assert
            Assert.AreEqual(2, objects.Length);
            CollectionAssert.Contains(objects, obj1);
            CollectionAssert.Contains(objects, obj2);
        }

        [TestMethod]
        public void Ids_ReturnsAllGuids()
        {
            // Arrange
            var id1 = _map.GetOrCreateId(new object());
            var id2 = _map.GetOrCreateId(new object());

            // Act
            var ids = _map.Ids;

            // Assert
            Assert.AreEqual(2, ids.Length);
            CollectionAssert.Contains(ids, id1);
            CollectionAssert.Contains(ids, id2);
        }
    }
}
