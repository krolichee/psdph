using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Views.SimpleView.Logic;

namespace psdPHTest.Logic.Parameters
{
	[TestClass]
	public class StringChooseParameterTest: ProjectTestSuite
    {
		

		[TestMethod]
		public void testSaveStrings()
		{

            
			//Directory.CreateDirectory(PsdPhDirectories.ProjectsDirectory);

			var blob = Blob.PathBlob("");
			var scParameter = new StringChooseParameter() { Name = "scPar" };
            scParameter.Strings.Add("1");
            blob.ParameterSet.Add(scParameter);

			project.saveBlob(blob);

			blob = project.openMainBlob();
			Assert.IsTrue((blob.ParameterSet.AsCollection()[0]as StringChooseParameter).Strings[0]=="1");

		}
		[TestMethod]
		public void testSimpleViewStringsEquality()
		{
			{
				var blob = Blob.PathBlob("");
				var scParameter = new StringChooseParameter() { Name = "scPar" };
				scParameter.Strings.Add("1");
				blob.ParameterSet.Add(scParameter);

				project.saveBlob(blob);
            }
			{
                var view = SimpleView.MakeSimpleView();
                var simpleListData = view.ListData;
				var blob = simpleListData.RootBlob;
                var scParameter = blob.ParameterSet.AsCollection()[0] as StringChooseParameter;

                simpleListData.New();
                var testedParameterSet = simpleListData.Variants[0].ParameterSet;
                var sld_scParameter = testedParameterSet.AsCollection()[0] as StringChooseParameter;

                SimpleView.Instance().Save();

                Assert.IsTrue((testedParameterSet.AsCollection()[0] as StringChooseParameter).Strings[0] == "1");
                Assert.IsTrue(sld_scParameter.Strings == scParameter.Strings);
            }
			

            

        }
		
	}
}
