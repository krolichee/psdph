using psdPH.Nodes;

namespace test.Nodes.Core
{
    class SingleBoolLetsNode : EmptyNode
    {
        bool bool1;
        public override Let[] Outlets => new Let[] {
            new Let(this,"kavabanga",typeof(bool),()=>bool1,(_)=>{ })
        };
        public override Let[] Inlets => new Let[] {
            new Let(this,"onobanga",typeof(bool),()=>false,(_)=>bool1 = (bool)_)
        };
    }
}
