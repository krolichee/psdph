using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Utils.ReflectionSetups
{
    [TestCategory(TestCatagories.ManualUI)]
    [TestClass]
    public class SetupTest
    {
        [TestMethod]
        public void testStringChoice()
        {
            var par = new StringChooseParameter() { Name = "uvu" };
            par.Strings = new ObservableCollection<string>() { "1", "2", "3" };
            var count = par.Strings.Count;
            var p_w = new SetupsInputWindow(par.Setups);
            p_w.ShowDialog();
            p_w = new SetupsInputWindow(par.Setups);
            p_w.ShowDialog();
            Assert.IsTrue(par.Strings.Count!=count);
        }
    }
}
