using psdPH.Lets;
using psdPH.Reflection;

namespace test.Nodes.Core
{
    class SingleBoolLetsNode : EmptyNode
    {
        public bool LetBool;
        public override Let[] Outlets => new Let[] {
            new Let(new ReflectionConfig(this,nameof(LetBool)))
        };
        public override Let[] Inlets => new Let[] {
            new Let(new ReflectionConfig(this,nameof(LetBool)))
        };
    }
}
