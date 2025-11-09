using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using psdPH.Serialization;

namespace test.Serialization
{
	/// <summary>
	/// Summary description for DtoTypesRegistryTest
	/// </summary>
	[TestClass]
	public class DtoTypesRegistryTest
	{

		[TestMethod]
		public void AddTest()
		{

            DtoTypesRegistry.Add(typeof(List<int>));
			Assert.IsTrue(DtoTypesRegistry.DtoTypes.Contains(typeof(List<int>)));

        }
	}
}
