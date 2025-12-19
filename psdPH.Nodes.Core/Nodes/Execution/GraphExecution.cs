using System.Collections.Generic;
using System.Linq;

namespace psdPH.Nodes.Core.Nodes
{
    class GraphExecution
    {
        readonly List<QueueCoherence> QueueCoherences;
        readonly Dictionary<Node, bool> NodesExecuted;
        readonly NodeGraph Graph;

        public GraphExecution(NodeGraph graph)
        {
            QueueCoherences = ExtractCoherences(graph).Select(c=>new QueueCoherence(c.From,c.To)).ToList();
            NodesExecuted = graph.Nodes.ToDictionary(n => n, _ => false);
            Graph = graph;
        }
        public Node[] GetReadyNodes()
        {
            return NonExecutedNodes().Where(n => IsAllCoherencesExecuted(n) && IsAllChainsLet(n)).ToArray();
        }
        public void TickExecuted(Node node)
        {
            foreach (var qc in QueueCoherences.Where(qc => qc.From == node))
            {
                qc.Executed = true;
            }
            NodesExecuted[node] = true;
        }
        IEnumerable<QueueCoherence> GetCoheredTo(Node node)
        {
            return QueueCoherences.Where(qc => qc.To == node);
        }
        bool IsAllCoherencesExecuted(Node node) => GetCoheredTo(node).All(qc => qc.Executed);

        IEnumerable<Node> NonExecutedNodes() => NodesExecuted.Where(kv => !kv.Value).Select(kv => kv.Key);

        IEnumerable<ChainLink> GetIngoingChains(Node node) => Graph.ChainLinks.Where(cl => cl.ToNode == node);
        bool IsAllChainsLet(Node node)
        {
            return GetIngoingChains(node).Select(cl => cl.FromLet.Let.Value).All(b => (bool)b);
        }

        static Coherence GetCoherence(NodeLetLink nodeLetLink)
        {
            return new Coherence(nodeLetLink.From.Node, nodeLetLink.To.Node);
        }
        static Coherence GetCoherence(ChainLink chainLink)
        {
            return new Coherence(chainLink.FromLet.Node, chainLink.ToNode);
        }
        static IEnumerable<Coherence> ExtractCoherences(NodeGraph graph)
        {
            var chainCoherences = graph.ChainLinks.Select(cl=>GetCoherence(cl));
            var straightCoherences = graph.NodeLinks.AsEnumerable();
            var letLinkCoherences = graph.NodeLetLinks.Select(nll=> GetCoherence(nll));
            List<Coherence> result = new List<Coherence>();
            return result.Concat(chainCoherences).Concat(straightCoherences).Concat(letLinkCoherences);
        }
        
        
    }
}
