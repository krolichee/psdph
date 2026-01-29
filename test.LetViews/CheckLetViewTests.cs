using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews;
using psdPH.LetViews.Check;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class CheckLetViewTests : AllLetViewTest
	{
        class TestObj
        {
            public bool Bool { get; set; }
        }
        protected override LetView GetLetView()
        {
            Let let = GetLet();
            var view = new CheckLetView(let);
            return view;
        }

        protected override ReflectionConfig GetConfig()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Bool));
            return config;
        }

        protected override void SetupLet(Let let)
        {
            let.Value = true;
        }
    }
}
