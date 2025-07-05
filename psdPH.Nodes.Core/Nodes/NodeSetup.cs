using psdPH.Setups;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public class NodeSetup
    {
        public Node Node;
        public Setup Setup;

        public NodeSetup(Node node, Setup setup)
        {
            Node = node;
            Setup = setup;
        }
        public void Link(NodeSetup otherSetupLink)
        {
            Node.Link(Setup, otherSetupLink.Node, otherSetupLink.Setup);

        }
        public void IsLinked()
        {
            Node.IsLinkedSetup(Setup);
        }
        public override bool Equals(object obj)
        {
            if (obj is NodeSetup nodeSetup)
                return nodeSetup.Node == Node && nodeSetup.Setup.Equals(Setup);
            return false;
        }
        public override int GetHashCode()
        {
            return Setup.GetHashCode() * 23 + Node?.GetHashCode() ?? 0;
        }

        public bool CanLink(NodeSetup nodeSetup)
        {
            return Node.CheckLink(Setup,nodeSetup.Setup);
        }
    }
}
