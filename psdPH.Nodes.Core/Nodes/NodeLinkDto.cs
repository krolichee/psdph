using psdPH.Setups;
using psdPH.Utils.Setups;
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
        public NodeSetupDescriptor(NodeSetupLink nodeSetup) : this(nodeSetup.Node, nodeSetup.Setup) { }

        public NodeSetupDescriptor()
        {
        }
    }
    public class NodeLinkDto
    {
        public NodeSetupDescriptor FromNodeDescriptor;
        public NodeSetupDescriptor ToNodeDescriptor;

        public NodeLinkDto()
        {
        }

        public NodeLinkDto(NodeSetupDescriptor from, NodeSetupDescriptor to)
        {
            FromNodeDescriptor = from;
            ToNodeDescriptor = to;
        }
    }
}
