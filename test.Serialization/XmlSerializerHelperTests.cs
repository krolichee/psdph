using System;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;

namespace test.Serialization
{

    [TestClass]
    public class XmlSerializerHelperTests
    {
        // Тестовые DTO классы
        [XmlRoot("TestPerson")]
        public class TestPerson
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public DateTime BirthDate { get; set; }
        }

        [XmlRoot("TestProduct")]
        public class TestProduct
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        public class TestClassWithArray
        {
            public string[] Items { get; set; }
            public int[] Numbers { get; set; }
        }

        public class TestClassWithList
        {
            public System.Collections.Generic.List<string> Names { get; set; }
        }

        #region GetXml Tests

        [TestMethod]
        public void GetXml_WithSimpleObject_ReturnsValidXml()
        {
            // Arrange
            var person = new TestPerson
            {
                Name = "John Doe",
                Age = 30,
                BirthDate = new DateTime(1990, 1, 1)
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(person);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<TestPerson"));
            Assert.IsTrue(xml.Contains("<Name>John Doe</Name>"));
            Assert.IsTrue(xml.Contains("<Age>30</Age>"));
        }

        [TestMethod]
        public void GetXml_WithNullObject_ThrowsArgumentNullException()
        {
            // Arrange
            object nullObj = null;

            // Act & Assert
            Assert.ThrowsException<NullReferenceException>(() =>
                XmlSerializerHelper.GetXml(nullObj));
        }

        [TestMethod]
        public void GetXml_WithEmptyObject_ReturnsValidXml()
        {
            // Arrange
            var emptyPerson = new TestPerson();

            // Act
            string xml = XmlSerializerHelper.GetXml(emptyPerson);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<TestPerson"));
            Assert.IsTrue(xml.Contains("<Name />"));
        }

        [TestMethod]
        public void GetXml_WithArrayProperty_ReturnsValidXml()
        {
            // Arrange
            var objWithArray = new TestClassWithArray
            {
                Items = new string[] { "item1", "item2", "item3" },
                Numbers = new int[] { 1, 2, 3 }
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(objWithArray);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<Items>"));
            Assert.IsTrue(xml.Contains("<string>item1</string>"));
            Assert.IsTrue(xml.Contains("<Numbers>"));
        }

        [TestMethod]
        public void GetXml_WithListProperty_ReturnsValidXml()
        {
            // Arrange
            var objWithList = new TestClassWithList
            {
                Names = new System.Collections.Generic.List<string> { "Alice", "Bob", "Charlie" }
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(objWithList);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<Names>"));
            Assert.IsTrue(xml.Contains("<string>Alice</string>"));
        }

        #endregion

        #region GetObj Tests (Generic)

        [TestMethod]
        public void GetObj_Generic_WithValidXml_ReturnsCorrectObject()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<TestPerson>
    <Name>Jane Smith</Name>
    <Age>25</Age>
    <BirthDate>1995-05-15T00:00:00</BirthDate>
</TestPerson>";

            // Act
            var result = XmlSerializerHelper.GetObj<TestPerson>(xml);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Jane Smith", result.Name);
            Assert.AreEqual(25, result.Age);
            Assert.AreEqual(new DateTime(1995, 5, 15), result.BirthDate);
        }

        [TestMethod]
        public void GetObj_Generic_WithEmptyXml_ThrowsException()
        {
            // Arrange
            string emptyXml = "";

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                XmlSerializerHelper.GetObj<TestPerson>(emptyXml));
        }

        [TestMethod]
        public void GetObj_Generic_WithNullXml_ThrowsException()
        {
            // Arrange
            string nullXml = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                XmlSerializerHelper.GetObj<TestPerson>(nullXml));
        }

        [TestMethod]
        public void GetObj_Generic_WithInvalidXml_ThrowsException()
        {
            // Arrange
            string invalidXml = "<InvalidXml><UnclosedTag>";

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                XmlSerializerHelper.GetObj<TestPerson>(invalidXml));
        }

        [TestMethod]
        public void GetObj_Generic_WithWrongTypeXml_ThrowsException()
        {
            // Arrange
            string productXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<TestProduct>
    <Id>123</Id>
    <Description>Test Product</Description>
    <Price>99.99</Price>
</TestProduct>";

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                XmlSerializerHelper.GetObj<TestPerson>(productXml));
        }

        #endregion

        #region GetObj Tests (Non-Generic)

        [TestMethod]
        public void GetObj_WithValidXmlAndType_ReturnsCorrectObject()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<TestProduct>
    <Id>456</Id>
    <Description>Another Product</Description>
    <Price>49.99</Price>
</TestProduct>";

            // Act
            var result = XmlSerializerHelper.GetObj(xml, typeof(TestProduct)) as TestProduct;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(456, result.Id);
            Assert.AreEqual("Another Product", result.Description);
            Assert.AreEqual(49.99m, result.Price);
        }

        [TestMethod]
        public void GetObj_WithNullType_ThrowsException()
        {
            // Arrange
            string xml = "<TestPerson><Name>Test</Name></TestPerson>";

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                XmlSerializerHelper.GetObj(xml, null));
        }

        [TestMethod]
        public void GetObj_WithArrayXml_ReturnsCorrectObject()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<TestClassWithArray>
    <Items>
        <string>first</string>
        <string>second</string>
    </Items>
    <Numbers>
        <int>1</int>
        <int>2</int>
    </Numbers>
</TestClassWithArray>";

            // Act
            var result = XmlSerializerHelper.GetObj(xml, typeof(TestClassWithArray)) as TestClassWithArray;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Items.Length);
            Assert.AreEqual("first", result.Items[0]);
            Assert.AreEqual(2, result.Numbers.Length);
            Assert.AreEqual(1, result.Numbers[0]);
        }

        #endregion

        #region RoundTrip Tests

        [TestMethod]
        public void RoundTrip_SimpleObject_ReturnsEquivalentObject()
        {
            // Arrange
            var original = new TestPerson
            {
                Name = "RoundTrip Test",
                Age = 40,
                BirthDate = new DateTime(1980, 6, 15)
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(original);
            var deserialized = XmlSerializerHelper.GetObj<TestPerson>(xml);

            // Assert
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(original.Name, deserialized.Name);
            Assert.AreEqual(original.Age, deserialized.Age);
            Assert.AreEqual(original.BirthDate, deserialized.BirthDate);
        }

        [TestMethod]
        public void RoundTrip_ComplexObject_ReturnsEquivalentObject()
        {
            // Arrange
            var original = new TestClassWithArray
            {
                Items = new string[] { "test1", "test2" },
                Numbers = new int[] { 10, 20, 30 }
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(original);
            var deserialized = XmlSerializerHelper.GetObj<TestClassWithArray>(xml);

            // Assert
            Assert.IsNotNull(deserialized);
            CollectionAssert.AreEqual(original.Items, deserialized.Items);
            CollectionAssert.AreEqual(original.Numbers, deserialized.Numbers);
        }

        [TestMethod]
        public void RoundTrip_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var original = new TestPerson
            {
                Name = "John & Jane <test> \"quote\"",
                Age = 35,
                BirthDate = DateTime.Now
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(original);
            var deserialized = XmlSerializerHelper.GetObj<TestPerson>(xml);

            // Assert
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(original.Name, deserialized.Name);
        }

        #endregion

        #region Edge Cases

        [TestMethod]
        public void GetXml_WithDateTimeMinValue_SerializesCorrectly()
        {
            // Arrange
            var person = new TestPerson
            {
                Name = "Test",
                Age = 1,
                BirthDate = DateTime.MinValue
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(person);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<BirthDate>0001-01-01"));
        }

        [TestMethod]
        public void GetXml_WithMaxValues_SerializesCorrectly()
        {
            // Arrange
            var product = new TestProduct
            {
                Id = int.MaxValue,
                Description = "Test",
                Price = decimal.MaxValue
            };

            // Act
            string xml = XmlSerializerHelper.GetXml(product);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains($"<Id>{int.MaxValue}</Id>"));
        }

        [TestMethod]
        public void GetObj_WithWhitespaceXml_ThrowsException()
        {
            // Arrange
            string whitespaceXml = "   ";

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                XmlSerializerHelper.GetObj<TestPerson>(whitespaceXml));
        }

        #endregion

        #region Integration Tests

        [TestMethod]
        public void MultipleSerializations_ProduceConsistentResults()
        {
            // Arrange
            var original = new TestPerson { Name = "Consistency Test", Age = 50 };

            // Act
            string xml1 = XmlSerializerHelper.GetXml(original);
            string xml2 = XmlSerializerHelper.GetXml(original);

            // Assert
            Assert.AreEqual(xml1, xml2);
        }

        [TestMethod]
        public void Serialization_WithDifferentObjects_ProducesDifferentXml()
        {
            // Arrange
            var person1 = new TestPerson { Name = "Person1", Age = 1 };
            var person2 = new TestPerson { Name = "Person2", Age = 2 };

            // Act
            string xml1 = XmlSerializerHelper.GetXml(person1);
            string xml2 = XmlSerializerHelper.GetXml(person2);

            // Assert
            Assert.AreNotEqual(xml1, xml2);
            Assert.IsTrue(xml1.Contains("Person1"));
            Assert.IsTrue(xml2.Contains("Person2"));
        }

        #endregion
    }
}