using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;

namespace psdPHTest.Logic.Ruleset.Rules.CoreRules
{
	[TestClass]
	[TestCategory(TestCatagories.Automatic)]
	public class FlagRuleTest
	{
		[DataTestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public void ParsetTest(bool result)
		{
			var flagParameter = new FlagParameter() { Name = "uvu"};
			var parset = new ParameterSet() { };
			parset.Add(flagParameter);
			var dummyCondition = new DummyCondition(result);
			var rule = new FlagRule() { Value = true,FlagParameter = flagParameter,Condition = dummyCondition};
			rule.SetParameterSet(parset);
			rule.CoreApply();
			Assert.IsTrue(flagParameter.Toggle == result);
		}
	}
}
