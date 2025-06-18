using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPHTest.Utils.ReflectionSetups
{
    [TestCategory(TestCatagories.ManualUI)]
    [TestClass]
    public class SetupTest
    {
        [TestMethod]
        public void testMargin()
        {
            var s = new StringParameter();
            var f = new FlagParameter();
            s.Name = "str";
            f.Name = "flag";
            var setups = new List<Setup>();
            setups.AddRange(s.Setups);
            setups.AddRange(f.Setups);
            var si_w = new SetupsInputWindow(setups.ToArray());
            si_w.ShowDialog();
        }
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
        [TestMethod]
        public void testRichString()
        {
            var s = new StringParameter();
            s.Name = "string";
            var config = new SetupConfig(s,nameof(s.Value),s.Name);
            var setups = new RichStringInputSetup(config);
            Window p_w;
            do
            {
                p_w = new SetupsInputWindow(setups);

            }
            while (p_w.ShowDialog() == true);
        }
    }
}
