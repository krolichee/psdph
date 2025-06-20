using System;
using System.Linq;
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
		[TestMethod]
		public void GuidLinkConversionTest()
		{
			var prot = new PrototypeBlob() { LayerName = "111",RelativeLayerName = "222" };
			var ph = new PlaceholderLeaf() { LayerName = "222", PrototypeBlob = prot };
			var root = new RootBlob();
			root.AddChild(prot);
			root.AddChild(ph);
			DiskOperations.SaveXml("test.xml",root);
			root = DiskOperations.LoadXml<RootBlob>("test.xml");
			prot = root.GetChildren<PrototypeBlob>().First();
			ph = root.GetChildren<PlaceholderLeaf>().First();
			Assert.IsTrue(ph.PrototypeBlob == prot);
		}
	}
}
