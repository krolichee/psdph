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

        internal static NodeLet Get(Let let)
        {
            if (!(let.Obj is Node))
                throw new ArgumentException();
            return new NodeLet(let.Obj as Node,let);
        }
    }
}
