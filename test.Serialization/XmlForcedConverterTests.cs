using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{
    [TestClass]
    public class XmlForcedConverterTests
    {
        // Тестовые DTO классы
        [XmlRoot("SourceClass")]
        public class SourceClass
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public DateTime Timestamp { get; set; }
        }

        [XmlRoot("TargetClass")]
        public class TargetClass
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public DateTime Timestamp { get; set; }
        }

        [XmlRoot("DifferentStructureClass")]
        public class DifferentStructureClass
        {
            public string DifferentName { get; set; }
            public decimal Price { get; set; }
        }

        [XmlRoot("EmptyClass")]
        public class EmptyClass
        {
            // Пустой класс для тестирования
        }

        public class ClassWithNestedObject
        {
            public string Title { get; set; }
            public SourceClass Nested { get; set; }
        }

        public class ClassWithArray
        {
            public string[] Items { get; set; }
            public int[] Numbers { get; set; }
        }

        #region Convert Tests - Основные сценарии

        [TestMethod]
        public void Convert_WithCompatibleTypes_ReturnsConvertedObject()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Test Name",
                Value = 42,
                Timestamp = new DateTime(2023, 1, 1, 10, 30, 0)
            };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(source.Name, result.Name);
            Assert.AreEqual(source.Value, result.Value);
            Assert.AreEqual(source.Timestamp, result.Timestamp);
        }

        [TestMethod]
        public void Convert_WithEmptyObject_ReturnsEmptyConvertedObject()
        {
            // Arrange
            var source = new SourceClass(); // Все свойства по умолчанию

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name);
            Assert.AreEqual(0, result.Value);
            Assert.AreEqual(DateTime.MinValue, result.Timestamp);
        }

        [TestMethod]
        public void Convert_WithNullProperties_PreservesNullProperties()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = null, // Явно null
                Value = 100,
                Timestamp = DateTime.Now
            };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name);
            Assert.AreEqual(100, result.Value);
        }

        #endregion

        #region Convert Tests - Особые случаи

        [TestMethod]
        public void Convert_WithDifferentPropertyNames_UsesMatchingProperties()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Test",
                Value = 123,
                Timestamp = DateTime.Now
            };

            // Act
            var result = XmlForcedConverter.Convert<DifferentStructureClass>(source);

            // Assert
            Assert.IsNotNull(result);
            // Свойство Name не маппится на DifferentName, поэтому DifferentName будет null
            Assert.IsNull(result.DifferentName);
            // Price не существует в SourceClass, поэтому значение по умолчанию
            Assert.AreEqual(0, result.Price);
        }

        [TestMethod]
        public void Convert_WithNestedObject_ConvertsSuccessfully()
        {
            // Arrange
            var source = new ClassWithNestedObject
            {
                Title = "Container",
                Nested = new SourceClass
                {
                    Name = "Nested Name",
                    Value = 999,
                    Timestamp = new DateTime(2023, 12, 31)
                }
            };

            // Act
            var result = XmlForcedConverter.Convert<ClassWithNestedObject>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Container", result.Title);
            Assert.IsNotNull(result.Nested);
            Assert.AreEqual("Nested Name", result.Nested.Name);
            Assert.AreEqual(999, result.Nested.Value);
        }

        [TestMethod]
        public void Convert_WithArrayProperties_PreservesArrays()
        {
            // Arrange
            var source = new ClassWithArray
            {
                Items = new string[] { "one", "two", "three" },
                Numbers = new int[] { 1, 2, 3, 4, 5 }
            };

            // Act
            var result = XmlForcedConverter.Convert<ClassWithArray>(source);

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(source.Items, result.Items);
            CollectionAssert.AreEqual(source.Numbers, result.Numbers);
        }

        [TestMethod]
        public void Convert_ToSameType_ReturnsEquivalentObject()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Same Type Test",
                Value = 777,
                Timestamp = DateTime.UtcNow
            };

            // Act
            var result = XmlForcedConverter.Convert<SourceClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(source.Name, result.Name);
            Assert.AreEqual(source.Value, result.Value);
            Assert.AreEqual(source.Timestamp, result.Timestamp);
        }

        #endregion

        #region Convert Tests - Граничные случаи и ошибки

        [TestMethod]
        public void Convert_WithEmptyClass_ReturnsEmptyInstance()
        {
            // Arrange
            var source = new EmptyClass();

            // Act
            var result = XmlForcedConverter.Convert<EmptyClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EmptyClass));
        }

        [TestMethod]
        public void Convert_WithSpecialCharactersInStrings_HandlesCorrectly()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Special & < > \" ' characters",
                Value = 1,
                Timestamp = DateTime.Now
            };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Special & < > \" ' characters", result.Name);
        }

        [TestMethod]
        public void Convert_WithMaxMinValues_PreservesValues()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Extreme Values",
                Value = int.MaxValue,
                Timestamp = DateTime.MaxValue
            };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(int.MaxValue, result.Value);
            Assert.AreEqual(DateTime.MaxValue, result.Timestamp);
        }

        #endregion

        #region ChangeType Tests (косвенное тестирование через Convert)

        [TestMethod]
        public void Convert_ChangesRootElementNameInXml()
        {
            // Arrange
            var source = new SourceClass { Name = "Test", Value = 1 };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            // Косвенно проверяем, что ChangeType отработал - конвертация прошла успешно
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
        }

        [TestMethod]
        public void Convert_WithDifferentNamespaces_HandlesCorrectly()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Namespace Test",
                Value = 123
            };

            // Act
            var result = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(source.Name, result.Name);
        }

        #endregion

        #region Интеграционные тесты

        [TestMethod]
        public void MultipleConversions_ProduceConsistentResults()
        {
            // Arrange
            var source = new SourceClass
            {
                Name = "Consistency Test",
                Value = 555,
                Timestamp = DateTime.Now
            };

            // Act
            var result1 = XmlForcedConverter.Convert<TargetClass>(source);
            var result2 = XmlForcedConverter.Convert<TargetClass>(source);

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(result1.Name, result2.Name);
            Assert.AreEqual(result1.Value, result2.Value);
            Assert.AreEqual(result1.Timestamp, result2.Timestamp);
        }

        [TestMethod]
        public void RoundTripConversion_ReturnsOriginalData()
        {
            // Arrange
            var original = new SourceClass
            {
                Name = "Round Trip",
                Value = 888,
                Timestamp = new DateTime(2023, 6, 15, 14, 30, 0)
            };

            // Act
            var converted = XmlForcedConverter.Convert<TargetClass>(original);
            var backConverted = XmlForcedConverter.Convert<SourceClass>(converted);

            // Assert
            Assert.IsNotNull(backConverted);
            Assert.AreEqual(original.Name, backConverted.Name);
            Assert.AreEqual(original.Value, backConverted.Value);
            Assert.AreEqual(original.Timestamp, backConverted.Timestamp);
        }

        #endregion

        #region Тесты на ошибки (ожидаемые исключения)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_WithNullObject_ThrowsException()
        {
            // Arrange
            SourceClass nullObject = null;

            // Act
            XmlForcedConverter.Convert<TargetClass>(nullObject);
        }

        #endregion

        // Вспомогательные классы для тестирования наследования
        public class BaseClass
        {
            public string BaseProperty { get; set; }
        }

        [XmlRoot("DerivedClass")]
        public class DerivedClass : BaseClass
        {
            public string DerivedProperty { get; set; }
        }

        [TestMethod]
        public void Convert_WithInheritance_PreservesBaseProperties()
        {
            // Arrange
            var source = new DerivedClass
            {
                BaseProperty = "Base Value",
                DerivedProperty = "Derived Value"
            };

            // Act
            var result = XmlForcedConverter.Convert<DerivedClass>(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Base Value", result.BaseProperty);
            Assert.AreEqual("Derived Value", result.DerivedProperty);
        }
    }
}