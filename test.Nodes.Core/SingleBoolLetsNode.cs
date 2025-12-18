using psdPH.Nodes;

namespace test.Nodes.Core
{
    class SingleBoolLetsNode : EmptyNode
    {
        public bool LetBool;
        public override Let[] Outlets => new Let[] {
            new Let(this,"kavabanga",typeof(bool),()=>LetBool,(_)=>{ })
        };
        public override Let[] Inlets => new Let[] {
            new Let(this,"onobanga",typeof(bool),()=>false,(_)=>LetBool = (bool)_)
        };
    }
}
