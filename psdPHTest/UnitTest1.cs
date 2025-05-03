using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;


namespace psdPHTest.Automatic
{
    [TestClass]
    public class DateFormat
    {

    }
    [TestClass]
    public class CompositionTest
    {

        [TestMethod]
        public void CompositionChildrenObserving()
        {
            bool eventRaised = false;
            void a()
            {
                eventRaised = true;
            }
            Composition composition = new Blob();
            composition.ChildrenUpdatedEvent += a;
            composition.addChild(new TextLeaf());
            Assert.IsTrue(eventRaised);
        }
        [TestMethod]
        public void RuleSetChildrenObserving()
        {
            bool eventRaised = false;
            void a()
            {
                eventRaised = true;
            }
            Composition composition = new Blob();
            composition.RulesetUpdatedEvent += a;
            composition.RuleSet.Rules.Add(new TextFontSizeRule());
            Assert.IsTrue(eventRaised);
        }
    }
    [TestClass]
    internal class ConcreteRuleTest
    {
        string testDirectory = "C:\\Users\\Puziko\\source\\repos\\psdPHTest\\psdPHTest";
        [TestMethod]
        public void Fitting()
        {
            Directories.BaseDirectory = testDirectory;
            Blob blob = PsdPhProject.openOrCreateMainBlob("Fitting");
            blob.addChild(new TextLeaf());
        }

    }
}
namespace psdPHTest.Manual
{

}
