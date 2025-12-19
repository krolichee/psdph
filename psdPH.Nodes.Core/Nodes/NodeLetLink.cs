namespace psdPH.Nodes
{
    //TODO наследовать от Coherence
    public class NodeLetLink
    {
        public NodeLet From { get; private set; }
        public NodeLet To { get; private set; }

        public NodeLetLink(NodeLet from, NodeLet to)
        {
            From = from;
            To = to;
        }

        public void Push()
        {
            To.Let.Value = From.Let.Value;
        }
    }
}
