using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public class NodeLetLink
    {
        public NodeLet From;
        public NodeLet To;

        public NodeLetLink(NodeLet from, NodeLet to)
        {
            From = from;
            To = to;
        }
    }
    public class NodeLet
    {
        public NodeLet(Node node, Let let)
        {
            Node = node;
            Let = let;
        }

        public Node Node { get; set; }
        public Let Let { get; set; }

        internal static NodeLet Get(Let let)
        {
            if (!(let.Obj is Node))
                throw new ArgumentException();
            return new NodeLet(let.Obj as Node,let);
        }
    }
    public class ChainLink
    {
        public NodeLet From;
        public Node ToNode;
        public bool Inverted;

        public ChainLink(NodeLet fromNodeLet, Node toNode, bool inverted)
        {
            From = fromNodeLet;
            ToNode = toNode;
            Inverted = inverted;
        }
    }
    public class NodeGraph
    {

        public List<NodeLetLink> NodeLetLinks { get; }
        public List<ChainLink> ChainLinks { get; }
        public List<Node> Nodes { get; }
        public Node RootNode { get; set; }

        public void Chain(Let fromLet, Node toNode, bool inverted)
        {
            if (fromLet.Type != typeof(bool))
                throw new ArgumentException();
            var fromNodeLet = NodeLet.Get(fromLet);
            var chain = new ChainLink(fromNodeLet, toNode, inverted);
            ChainLinks.Add(chain);
        }

        public void Link(Let fromLet, Let toLet)
        {
            var from = NodeLet.Get(fromLet);
            var to = NodeLet.Get(toLet);
            var link = new NodeLetLink(from,to);
            NodeLetLinks.Add(link);
        }
    }
}
