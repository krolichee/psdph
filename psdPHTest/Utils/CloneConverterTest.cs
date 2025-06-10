using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Utils;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPHTest.Utils
{
    [TestCategory(TestCatagories.Automatic)]
    [TestClass]
    public class CloneConverterTest
    {
        [TestMethod]
        public void testListDerivedSerialization()
        {
            var dow = DayOfWeek.Tuesday;
            var obj = new DayParameterSet(dow,0);
            var clone = CloneConverter.Clone(obj) as DayParameterSet;
            Assert.IsTrue(clone.Dow == dow);
        }
    }
}
