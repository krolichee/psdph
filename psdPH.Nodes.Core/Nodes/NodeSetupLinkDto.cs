using psdPH.Utils.Setups;

namespace psdPH.Nodes
{
    public class NodeSetupLinkDto
    {
        public NodeSetupDescriptor FromNodeDescriptor;
        public NodeSetupDescriptor ToNodeDescriptor;

        public NodeSetupLinkDto()
        {
        }

        public NodeSetupLinkDto(NodeSetupDescriptor from, NodeSetupDescriptor to)
        {
            FromNodeDescriptor = from;
            ToNodeDescriptor = to;
        }
    }
}
