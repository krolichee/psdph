using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using psdPH.Lets;
using psdPH.Nodes;
using psdPH.Reflection;

namespace psdPH.Nodes.Basic
{
    public class TernarNode : Node
    {
        public Let FactorLet { get; }
        public Let TrueLet{get;}
        public Let FalseLet{get;}
        public Let OutputLet{get;}

        public TernarNode()
        {
            FactorLet = Let.FromField(this, nameof(Factor));
            TrueLet = Let.FromField(this, nameof(TrueVariant));
            FalseLet = Let.FromField(this, nameof(FalseVariant));
            OutputLet = Let.FromField(this, nameof(Output));
        }

        public override Let[] Inlets => new[] {
             FactorLet,
             TrueLet,
             FalseLet,
            };

        public override Let[] Outlets => new[] { OutputLet };

        public bool Factor { get; set; }
        public object TrueVariant { get; set; }
        public object FalseVariant { get; set; }
        public object Output { get; internal set; }

        public override void Execute()
        {
            Output = Factor ? TrueVariant : FalseVariant;
        }
    }
}
