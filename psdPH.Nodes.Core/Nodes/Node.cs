using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using psdPH.Lets;

namespace psdPH.Nodes
{
    abstract public class Node : INodable
    {
        bool canBeExecuted = true;

        protected Node()
        {
            Flowlet = Let.FromField(this,nameof(Flow));
        }

        bool Flow { get => canBeExecuted; set => canBeExecuted &= value; }
        public Let Flowlet { get; private set; }
        public abstract IEnumerable<Let> Inlets { get; }
        public abstract IEnumerable<Let> Outlets { get; }
        protected abstract void execute();
        public void Execute()
        {
            if (canBeExecuted)
                execute();
        }
    }
}
