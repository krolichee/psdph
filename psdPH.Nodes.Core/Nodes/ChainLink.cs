namespace psdPH.Nodes
{
    //TODO наследовать от Coherence
    public class ChainLink
    {
        public NodeLet FromLet;
        public Node ToNode;
        public bool Inverted;

        public ChainLink(NodeLet fromNodeLet, Node toNode, bool inverted)
        {
            FromLet = fromNodeLet;
            ToNode = toNode;
            Inverted = inverted;
        }
    }
}
