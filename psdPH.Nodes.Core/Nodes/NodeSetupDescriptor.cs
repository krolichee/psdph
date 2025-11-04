using psdPH.Setups;
using System;

namespace psdPH.Nodes
{
    public class NodeSetupDescriptor
    {
        public Guid NodeGuid;
        public int SetupConfigHash;
        public NodeSetupDescriptor(Node node, Setup setup)
        {
            NodeGuid = node.Guid;
            SetupConfigHash = setup.GetHashCode();
        }
        public NodeSetupDescriptor(NodeSetup nodeSetup) : this(nodeSetup.Node, nodeSetup.Setup) { }

        public NodeSetupDescriptor()
        {
        }
    }
}
