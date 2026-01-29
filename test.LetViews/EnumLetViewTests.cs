using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class EnumLetViewTests : AllLetViewTest
	{
 
        class TestObj
        {
            public ExecutionScope Scope;
        }

        protected override LetView GetLetView()
        {
            Let let = GetLet();
            var view = new EnumLetView(let);
            return view;
        }

        protected override ReflectionConfig GetConfig()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Scope));
            return config;
        }

        protected override void SetupLet(Let let)
        {
            let.Value = ExecutionScope.ClassLevel;
        }
    }
}
