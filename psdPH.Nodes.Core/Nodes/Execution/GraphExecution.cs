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
            QueueCoherences = graph.GetCoherences().Select(c=>new QueueCoherence(c)).ToList();
            NodesExecuted = graph.Nodes.ToDictionary(n => n, _ => false);
            Graph = graph;
        }
        public IEnumerable<Node> GetReadyNodes()
        {
            return NonExecutedNodes().Where(n => IsAllCoherencesExecuted(n) && IsAllChainsLet(n));
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
            return QueueCoherences.GetCoheredTo(node).Select(c=>c as QueueCoherence);
        }
        bool IsAllCoherencesExecuted(Node node) => GetCoheredTo(node).All(qc => qc.Executed);

        IEnumerable<Node> NonExecutedNodes() => NodesExecuted.Where(kv => !kv.Value).Select(kv => kv.Key);

        IEnumerable<NodeLetLink> GetIngoingChains(Node node) => Graph.Links.Where( cl => cl.To.Node == node && cl.IsChain());
        bool IsAllChainsLet(Node node)
        {
            return GetIngoingChains(node).Select(cl => cl.From.Let.Value).All(b => (bool)b);
        }

        
        
        
        
    }
}
