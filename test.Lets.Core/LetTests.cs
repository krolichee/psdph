using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.Setups;

namespace test.Lets.Core
{
    [TestClass]
    public class LetTests
    {
        class TestObj
        {
           public int A { get; set; }
        }
        [TestMethod]
        public void ReflectionConfig_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(obj.A));
            var let = new Let(config);
            const int NEW_VALUE = 2;
            let.Value = NEW_VALUE;
            Assert.IsTrue(obj.A == NEW_VALUE);
        }

    }
}
