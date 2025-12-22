using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;

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
        public void GetSetConstructor_test()
        {
            var obj = new TestObj();
            var let = new Let(obj, "ololo", obj.GetType(),()=>obj.A,(a)=>obj.A =  (int)a );
            const int NEW_VALUE = 2;
            let.Value = NEW_VALUE;
            Assert.IsTrue(obj.A == NEW_VALUE);
        }
        [TestMethod]
        public void MemberConstructor_test()
        {
            var obj = new TestObj();
            var let = new Let(obj,)
        }

    }
}
