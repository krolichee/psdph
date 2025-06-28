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
        [XmlElement(Order = 1)]
        public ObservableCollection<Node> Nodes = new ObservableCollection<Node>();
        
        void CalculateLinks()
        {
            var result = new List<NodeLinkDto>();
            foreach (var node in Nodes)
            {
                NodeSetupDescriptor from;
                NodeSetupDescriptor to;
                foreach (var outputLink in node.OutputLinks)
                {
                    from = new NodeSetupDescriptor(node, outputLink.Key);
                    to = new NodeSetupDescriptor(outputLink.Value);
                    result.Add(new NodeLinkDto(from, to));
                }

            }
            Links = result;
        }
        void ApplyLinks()
        {
            foreach (var nodeLinkDto in Links)
            {
                var from = nodeLinkDto.FromNodeDescriptor;
                var to = nodeLinkDto.ToNodeDescriptor;
                var fromNode = GuidScope.Current.GetByGuid(from.NodeGuid) as Node;
                var toNode = GuidScope.Current.GetByGuid(to.NodeGuid) as Node;
                var fromSetup = fromNode.IOSetups.First(s => s.GetHashCode() == from.SetupConfigHash);
                var toSetup = toNode.IOSetups.First(s => s.GetHashCode() == to.SetupConfigHash);
                fromNode.Link(fromSetup, toNode, toSetup);
            }
        }
        
        class IdiNahuyException : Exception
        {

        }
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
        public List<NodeLinkDto> Links;
        [XmlElement(Order = 4)]
        public bool onDeserializing
        {
            get => true;
            set
            {
                GuidScope.Current.GuidsLoaded+=ApplyLinks;
            }
        }
        public Node this[int index]
        {
            get => Nodes[index];
            set => Nodes[index] = value;
        }
    }
}
