using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews;
using psdPH.LetViews.Choose;
using psdPH.Reflection;

namespace test.LetViews
{
	[TestClass]
	public class DateLetViewTests : AllLetViewTest
	{
        
        class TestObj
        {
            public DateTime Date;
        }

        protected override LetView GetLetView()
        {    
            Let let = GetLet();
            var view = new DateLetView(let);
            return view;
        }

        protected override ReflectionConfig GetConfig()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Date));
            return config;
        }

        protected override void SetupLet(Let let)
        {
            let.Value = new DateTime(2077, 12, 25);
        }
    }
}
