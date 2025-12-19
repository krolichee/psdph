namespace psdPH.Nodes.Core.Nodes
{
    class QueueCoherence : Coherence
    {
        public QueueCoherence(Coherence coherence) : base(coherence.From, coherence.To) { }
        public QueueCoherence(Node from, Node to) : base(from, to)
        {
            Executed = false;
        }

        public bool Executed { get; set; }
    }
}
