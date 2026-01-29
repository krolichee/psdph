using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews.Check;
using psdPH.LetViews.Choose;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class ChooseLetViewTests : AllLetViewTest
	{
        TestObj obj;
        class TestObj
        {
            public int Selected { get; set; }
            public int[] Options => new int[] { 1, 2, 3, 4 };
        }

        protected override LetView GetLetView()
        {
            Let let = GetLet();
            var view = new ChooseLetView(let, obj.Options.Cast<object>().ToArray());
            return view;
        }

        protected override ReflectionConfig GetConfig()
        {
            obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Selected));
            return config;
        }

        protected override void SetupLet(Let let)
        {
         obj.Selected = 5;
        }
    }
}
