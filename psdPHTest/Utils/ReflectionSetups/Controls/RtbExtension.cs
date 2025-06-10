using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Utils.ReflectionParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPHTest.Utils.ReflectionSetups.Controls
{
    [TestCategory(TestCatagories.Automatic)]
    [TestClass]
    public class RtbExtensionTest
    {
        [TestMethod]
        public void stringEquality()
        {
            Assert.AreEqual("aaa","aaa".Replace("d","d"));
        }
        [DataTestMethod]
        [DataRow("aaa")]
        [DataRow("a  a      a")]
        [DataRow("a \n a  \n a")]
        [DataRow("a \r a  \r a")]
        [DataRow("a \t a_ds фывыфв  \t a")]
        [DataRow("в лесу родилась елочка \n в лесу она росла")]
        public void ConverRevertEquality(string text)
        {
           var rtb = new RichTextBox();
            rtb.SetText(text);
            Assert.AreEqual(rtb.GetText(),text);
          
        }
    }
}
