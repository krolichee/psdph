namespace psdPH.Nodes.Core.Nodes
{
    class QueueCoherence : Coherence
    {
        public QueueCoherence(Node from, Node to) : base(from, to)
        {
            Executed = false;
        }

        public bool Executed { get; set; }
    }
}
