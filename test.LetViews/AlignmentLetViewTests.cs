using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.LetViews;
using psdPH.Reflection;

namespace test.LetViews
{
    [TestClass]
    public class AlignmentLetViewTests
    {
        Let let;
        class AlignmentTestObj
        {
            public Alignment Alignment { get; set; }
        }
        [TestMethod]
        public void Usage_test()
        {
            var obj = new AlignmentTestObj();
            var config = new ReflectionConfig(obj, nameof(AlignmentTestObj.Alignment));
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
