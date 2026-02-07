using psdPH.Lets;
using System;

namespace psdPH.Nodes
{
    public class NodeLet
    {
        public NodeLet(Node node, Let let)
        {
            Node = node;
            Let = let;
        }

        public Node Node { get; set; }
        public Let Let { get; set; }

        public static NodeLet Get(Let let)
        {
            if (!(let.Obj is Node))
                throw new ArgumentException();
            return new NodeLet(let.Obj as Node,let);
        }
        public bool IsFlowlet()
        {
            return Let == Node.Flowlet;
        }
        public override bool Equals(object obj)
        {
            if (obj is NodeLet other)
                return other.Node.Equals(Node) && other.Let.Equals(Let);
            return false;
        }
    }
}
