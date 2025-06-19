using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Utils;

namespace psdPHTest.Logic
{
	[TestCategory(TestCatagories.Automatic)]
	[TestClass]
	public class DtoConverter
	{
		[TestMethod]
		public void TextLeafConversionTest()
		{
			var obj = new TextLeaf() { LayerName = "111"};
			var xml = CloneConverter.GetXml(obj);
			Console.WriteLine(xml);
			obj = CloneConverter.GetObj<TextLeaf>(xml) ;
			Assert.IsTrue(obj.LayerName == "111");
		}
	}
}
