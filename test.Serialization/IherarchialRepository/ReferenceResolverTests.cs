using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization.IherarchialRepository
{
    [TestClass]
    public class ReferenceResolverTests
    {
        private ConversionContext _context;
        private List<PendingReference> _pendingReferences;
        private IdentityMap _identityMap;

        [TestInitialize]
        public void Setup()
        {
            _identityMap = new IdentityMap();
            _pendingReferences = new List<PendingReference>();
        }

        [TestMethod]
        public void ResolveReferences_ValidReferences_ResolvesAllReferences()
        {
            // Arrange
            var targetObject = new object();
            var targetGuid = Guid.NewGuid();
            _identityMap.AddMapping(targetObject, targetGuid);

            var wasSetter1Called = false;
            var wasSetter2Called = false;

            _pendingReferences.Add(new PendingReference
            {
                TargetEntityGuid = targetGuid,
                ReferenceSetter = obj => { wasSetter1Called = true; Assert.AreSame(targetObject, obj); }
            });

            _pendingReferences.Add(new PendingReference
            {
                TargetEntityGuid = targetGuid,
                ReferenceSetter = obj => { wasSetter2Called = true; Assert.AreSame(targetObject, obj); }
            });

            _context = new ConversionContext(_identityMap, _pendingReferences.ToArray());

            // Act
            ReferenceResolver.ResolveReferences(_context);

            // Assert
            Assert.IsTrue(wasSetter1Called);
            Assert.IsTrue(wasSetter2Called);
        }

        [TestMethod]
        public void ResolveReferences_MissingTarget_ThrowsKeyNotFoundException()
        {
            // Arrange
            var missingGuid = Guid.NewGuid();
            _pendingReferences.Add(new PendingReference
            {
                TargetEntityGuid = missingGuid,
                ReferenceSetter = obj => { }
            });

            _context = new ConversionContext(_identityMap, _pendingReferences.ToArray());

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => ReferenceResolver.ResolveReferences(_context));
        }

        [TestMethod]
        public void ResolveReferences_NullContext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => ReferenceResolver.ResolveReferences(null));
        }

        [TestMethod]
        public void ResolveReferences_NullPendingReferences_DoesNotThrow()
        {
            // Arrange
            _context = new ConversionContext(_identityMap, null);

            // Act & Assert
            ReferenceResolver.ResolveReferences(_context); // Не должно бросать исключение
        }

        [TestMethod]
        public void ResolveReferences_EmptyPendingReferences_DoesNotThrow()
        {
            // Arrange
            _context = new ConversionContext(_identityMap, new PendingReference[0]);

            // Act & Assert
            ReferenceResolver.ResolveReferences(_context); // Не должно бросать исключение
        }
    }
}
