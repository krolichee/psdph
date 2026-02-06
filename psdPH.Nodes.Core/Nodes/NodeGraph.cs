using psdPH.Lets;
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
        readonly List<NodeLetLink> nodeLetLinks;
        readonly List<ChainLink> chainLinks;
        readonly List<Coherence> nodeLinks;
        readonly List<Node> nodes;
        public NodeGraph()
        {
            nodeLetLinks = new List<NodeLetLink>();
            chainLinks = new List<ChainLink>();
            nodes = new List<Node>();
            nodeLinks = new List<Coherence>();
        }

        public IEnumerable<NodeLetLink> NodeLetLinks => nodeLetLinks;
        public IEnumerable<ChainLink> ChainLinks => chainLinks;
        //TODO NodeCoherence dict (from-to,to-from) cache
        //TODO calculated coherences
        public IEnumerable<Coherence> NodeLinks => nodeLinks;
        public List<Node> Nodes => nodes;

        public void Chain(Let fromLet, Node toNode, bool inverted=false)
        {
            if (fromLet.Type != typeof(bool))
                throw new ArgumentException();
            var fromNodeLet = NodeLet.Get(fromLet);
            var chain = new ChainLink(fromNodeLet, toNode, inverted);
            chainLinks.Add(chain);
        }

        public void LetLink(Let fromLet, Let toLet)
        {
            var from = NodeLet.Get(fromLet);
            var to = NodeLet.Get(toLet);
            LetLink(from, to);
        }
        public void LetLink(NodeLet from, NodeLet to)
        {
            var link = new NodeLetLink(from, to);
            nodeLetLinks.Add(link);
        }
        public void NodeLink(Node from, Node to)
        {
            var coherence = new Coherence(from,to);
            nodeLinks.Add(coherence);
        }
        public void DeleteNode(Node node)
        {
            nodeLetLinks.RemoveAll((nl)=>nl.From.Node==node|| nl.To.Node == node);
            chainLinks.RemoveAll((nl)=>nl.FromLet.Node==node|| nl.ToNode == node);
            nodeLinks.RemoveAll((nl)=>nl.FromLet.Node==node|| nl.ToNode == node);
        }
    }
}
