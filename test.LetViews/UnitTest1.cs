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
    public class UnitTest1
    {
        Let let;
        class TestObj
        {
            public Alignment Alignment { get; set; }
        }
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new TestObj();
            var config = new ReflectionConfig(obj, nameof(TestObj.Alignment));
            let = new Let(config);
            var view = new AlignmentLetView(let);
            var viewModel = view.Control.DataContext as AlignmentLetViewModel;
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
