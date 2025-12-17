using System;
using psdPH.Nodes;

namespace test.Nodes.Core
{
    class EmptyNode : Node
    {
        public override Let[] Inlets => new Let[0];

        public override Let[] Outlets => new Let[0];

        public override Let[] Chain => new Let[0];

        public override void Execute(psdPH.Photoshop.DocumentWr doc)
        {
            
        }
    }
}
