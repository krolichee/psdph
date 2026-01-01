using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.Photoshop;
using psdPH.Reflection;

namespace test.LetViews
{
    [TestClass]
    public class AlignmentLetViewTests
    {
        Let let;
        class TestObj
        {
            public Alignment Alignment { get; set; }
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Alignment));
            let = new Let(config);
            let.Value = new Alignment(HAilgnment.Left, VAilgnment.Top);
            var view = new AlignmentLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
