using System.Security.RightsManagement;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews;
using psdPH.Reflection;

namespace test.LetViews
{
    [TestClass]
    public class AlignmentLetViewTests : AllLetViewTest
    {
        class TestObj
        {
            public Alignment Alignment { get; set; }
        }
        protected override LetView GetLetView()
        { 
            Let let = GetLet(); 
            var view = new AlignmentLetView(let);
            return view;
        }
        
        protected override ReflectionConfig GetConfig()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Alignment));
            return config;
        }

        protected override void SetupLet(Let let)
        {
            let.Value = new Alignment(HAilgnment.Left, VAilgnment.Top);
        }
    }
}
