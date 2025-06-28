using psdPH.Setups;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public class NodeSetupLink
    {
        public Node Node;
        public Setup Setup;

        public NodeSetupLink(Node node, Setup setup)
        {
            Node = node;
            Setup = setup;
        }
    }
}
