using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Setups;

namespace test.Reflection
{
    [TestClass]
    public class ReflectionConfigTests
    {
        class TestObj
        {
            public int A { get; set; }
            public int B;
            protected int C;
        }
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new TestObj();
            var configA = new ReflectionConfig(obj,nameof(obj.A));
            var configB = new ReflectionConfig(obj, nameof(obj.B));
            configA.SetValue(1);
            configB.SetValue(2);
            Assert.IsTrue(obj.A == 1);
            Assert.IsTrue(obj.B == 2);
        }
    }
}
