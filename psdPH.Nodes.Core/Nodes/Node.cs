using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace psdPH.Nodes
{
    abstract public class Node : INodable
    {
        public abstract Let[] Inlets { get; }
        public abstract Let[] Outlets { get; }
        public abstract void Execute();
    }
}
