using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public class NodeGraph
    {
        
        public NodeGraph()
        {
            NodeLetLinks = new List<NodeLetLink>();
            ChainLinks = new List<ChainLink>();
            Nodes = new List<Node>();
            RootNode = null;
            NodeLinks = new List<Coherence>();
        }
        
        public List<NodeLetLink> NodeLetLinks { get; }
        public List<ChainLink> ChainLinks { get; }
        //TODO NodeCoherence dict (from-to,to-from) cache
        //TODO calculated coherences
        public List<Coherence> NodeLinks { get; }
        public List<Node> Nodes { get; }
        public Node RootNode { get; set; }

        public void Chain(Let fromLet, Node toNode, bool inverted=false)
        {
            if (fromLet.Type != typeof(bool))
                throw new ArgumentException();
            var fromNodeLet = NodeLet.Get(fromLet);
            var chain = new ChainLink(fromNodeLet, toNode, inverted);
            ChainLinks.Add(chain);
        }

        public void LetLink(Let fromLet, Let toLet)
        {
            var from = NodeLet.Get(fromLet);
            var to = NodeLet.Get(toLet);
            var link = new NodeLetLink(from,to);
            NodeLetLinks.Add(link);
        }
        public void NodeLink(Node from, Node to)
        {
            var coherence = new Coherence(from,to);
            NodeLinks.Add(coherence);
        }
    }
}
