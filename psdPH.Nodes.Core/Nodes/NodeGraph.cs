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
        readonly List<NodeLetLink> links = new List<NodeLetLink>();
        readonly List<Node> nodes = new List<Node>();
        public NodeGraph()
        {
        }
        //TODO NodeCoherence dict (from-to,to-from) cache
        //TODO calculated coherences
        public IEnumerable<NodeLetLink> Links => links;
        public List<Node> Nodes => nodes;

        public bool IsLinked(Node node1, Node node2)
        {
            return links.Where((l) => l.From.Node == node1).Any((l) => l.To.Node == node2);
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
            if (to.Let == to.Node.Flowlet)
                if (from.Let.Type != typeof(bool))
                    throw new ArgumentException();
            links.Add(link);
        }
        public void DeleteNode(Node node)
        {
            links.RemoveAll((nl) => nl.From.Node == node || nl.To.Node == node);
            nodes.Remove(node);
        }

        public void DeleteLink(NodeLetLink link)
        {
            links.Remove(link);
        }
    }
}
