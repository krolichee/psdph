using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;

namespace psdPHTest.Tests.UI
{
    [TestCategory(TestCategories.ManualUI)]
    [TestClass]
    public class SetupTest
    {
        public object[] Objects;
        public string str=string.Empty;
        [TestMethod]
        public void testMulti()
        {
            var options = new string[] { "1", "2", "3" };
            var cfg = new ReflectionConfig(this,nameof(Objects),"каво");
            var parameters = new Setup[] { new MultiChooseSetup(cfg, options)};
            var pi_w = new SetupsInputWindow(parameters);
            pi_w.ShowDialog();
            Assert.IsTrue(Objects[0] as string=="1");
            Assert.IsTrue(Objects[1] as string=="3");
        }
        [TestMethod]
        public void testRtb()
        {
            var cfg = new ReflectionConfig(this, nameof(str), "каво");
            var parameters = new Setup[] { new RichStringInputSetup(cfg) };
            while (new SetupsInputWindow(parameters).ShowDialog() == true) ;
        }

    }
}
