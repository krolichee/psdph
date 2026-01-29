using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPHTest.Project;


namespace psdPHTest.Parameters
{
	[TestCategory(TestCategories.AutomaticDisk)]
	[TestClass]
	public class StringChooseParameterTest: ProjectTestSuite
    {
		

		[TestMethod]
		public void testSaveStrings()
		{
            
			//Directory.CreateDirectory(PsdPhDirectories.ProjectsDirectory);

			var blob = new RootBlob();
			var scParameter = new StringChooseParameter() { Name = "scPar" };
            scParameter.Strings.Add("1");
            blob.ParameterSet.Add(scParameter);

			project.saveBlob(blob);

			blob = project.openMainBlob();
			Assert.IsTrue((blob.ParameterSet.AsCollection()[0]as StringChooseParameter).Strings[0]=="1");
		}
		
	}
}
