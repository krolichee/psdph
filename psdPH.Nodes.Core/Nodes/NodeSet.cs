using psdPH.Logic.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;


namespace psdPH.Nodes.Core
{
    public class NodeSet :  ISerializable
    {
        
        
        void CalculateLinks()
        {
            var result = new List<NodeSetupLinkDto>();
            foreach (var node in Nodes)
            {
                NodeSetupDescriptor from;
                NodeSetupDescriptor to;
                foreach (var outputLink in node.Links)
                {
                    from = new NodeSetupDescriptor(outputLink.FromNodeSetup);
                    to = new NodeSetupDescriptor(outputLink.ToNodeSetup);
                    result.Add(new NodeSetupLinkDto(from, to));
                }

            }
            SetupLinks = result;
        }
        void ApplyLinks()
        {
            foreach (var nodeLinkDto in SetupLinks)
            {
                var from = nodeLinkDto.FromNodeDescriptor;
                var to = nodeLinkDto.ToNodeDescriptor;
                var fromNode = GuidScope.Current.GetByGuid(from.NodeGuid) as Node;
                var toNode = GuidScope.Current.GetByGuid(to.NodeGuid) as Node;
                var fromSetup = fromNode.IOSetups.First(s => s.GetHashCode() == from.SetupConfigHash);
                var toSetup = toNode.IOSetups.First(s => s.GetHashCode() == to.SetupConfigHash);
                fromNode.LinkOut(fromSetup, toNode, toSetup);
            }
        }
        
        [XmlElement(Order = 1)]
        public ObservableCollection<Node> Nodes = new ObservableCollection<Node>();
        [XmlElement(Order = 2)]
        public bool onSerializing
        {
            get
            {
                CalculateLinks();
                return true;
            }
            set { }
        }
        [XmlElement(Order = 3)]
        public List<NodeSetupLinkDto> SetupLinks;
        [XmlElement(Order = 4)]
        public List<NodeSetupLinkDto> ChainLinks;
        [XmlElement(Order = 5)]
        public bool onDeserializing
        {
            get => true;
            set
            {
                void onGuidsLoaded()
                {
                    GuidScope.Current.GuidsLoaded -= onGuidsLoaded;
                    ApplyLinks();
                }
                GuidScope.Current.GuidsLoaded += onGuidsLoaded;
            }
        }
        public Node this[int index]
        {
            get => Nodes[index];
            set => Nodes[index] = value;
        }
    }
}
