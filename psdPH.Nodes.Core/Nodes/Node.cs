using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;
using System.Collections.ObjectModel;
using psdPH.Photoshop;

namespace psdPH.Nodes
{
    abstract public class Node
    {
        public abstract Let[] Inlets { get; }
        public abstract Let[] Outlets { get; }
        public abstract Let[] Chain { get; }
        public abstract void Execute(DocumentWr doc);
    }
}
