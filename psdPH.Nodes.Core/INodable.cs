using psdPH.Lets;
using System.Collections.Generic;

namespace psdPH.Nodes
{
    public interface INodable
    {
        IEnumerable<Let> Inlets { get; }
        IEnumerable<Let> Outlets { get; }

        void Execute();
    }
}