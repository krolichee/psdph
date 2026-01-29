using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;
using psdPH.Utils;

namespace test.Serialization
{
	[TestClass]
	public class IhererchialRepositoryTests
	{
		[TestMethod]
		public void WriteAndRead()
		{
			DtoTypesRegistrator.InitializeRegistry();
            DtoConverterRegistrator.InitializeRegistry();
            var path = Path.GetTempFileName();
			var entity = new RefEntity();
			var ref1 = new SimpleEntity(1);
			var ref2 = new SimpleEntity(2);
			entity.Ref1 = ref1;
			entity.Ref2 = ref2;

			IherarchialRepository.WriteRoot(path,entity);
			entity = IherarchialRepository.ReadRoot(path) as RefEntity;

			Assert.IsTrue(ref1.Equals(entity.Ref1));
			Assert.IsTrue(ref2.Equals(entity.Ref2));
			File.Delete(path);
		}
	}
}
