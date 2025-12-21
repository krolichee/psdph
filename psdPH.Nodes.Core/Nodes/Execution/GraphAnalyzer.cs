using psdPH.Nodes.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public static class GraphAnalyzer
    {
        static class GraphExceptions
        {
            public const string CYCLED_GRAPH = "Cycled graph";
            public const string MULTI_ROOTS = "Graph has multiple root nodes";
        }

        static Coherence GetCoherence(NodeLetLink nodeLetLink)
        {
            return new Coherence(nodeLetLink.From.Node, nodeLetLink.To.Node);
        }
        static Coherence GetCoherence(ChainLink chainLink)
        {
            return new Coherence(chainLink.FromLet.Node, chainLink.ToNode);
        }
        public static IEnumerable<Coherence> GetCoherences(this NodeGraph graph)
        {
            var chainCoherences = graph.ChainLinks.Select(cl => GetCoherence(cl));
            var straightCoherences = graph.NodeLinks.AsEnumerable();
            var letLinkCoherences = graph.NodeLetLinks.Select(nll => GetCoherence(nll));
            List<Coherence> result = new List<Coherence>();
            return result.Concat(chainCoherences).Concat(straightCoherences).Concat(letLinkCoherences);
        }
        public static IEnumerable<Node> GetSourceNodes(this NodeGraph graph)
        {
            return graph.Nodes.Where(n => n is SourceNode);
        }
        public static Node GetRootNode(this NodeGraph graph)
        {
            var rootNodes = GetRootishNodes(graph).Except(GetSourceNodes(graph));
            if (rootNodes.Count() > 1)
                throw new ArgumentException(GraphExceptions.MULTI_ROOTS);
            return rootNodes.First();
        }
        public static IEnumerable<Node> GetRootishNodes(this NodeGraph graph)
        {
            var coherences = GetCoherences(graph);
            return graph.Nodes.Where(n => !GetCoheredTo(coherences, n).Any());
        }
        public static IEnumerable<Coherence> GetCoheredTo(this IEnumerable<Coherence> coherences, Node node)
        {
            return coherences.Where(c => c.To == node);
        }
        public static IEnumerable<Coherence> GetCoheredFrom(this IEnumerable<Coherence> coherences, Node node)
        {
            return coherences.Where(qc => qc.From == node);
        }
        public static bool IsCycled(this NodeGraph graph)
        {


            Dictionary<Node, bool> nodesWalked = graph.Nodes.ToDictionary(n => n, _ => false);
            var coherences = GetCoherences(graph);

            void walkFrom(Node node)
            {
                if (nodesWalked[node])
                    throw new Exception(GraphExceptions.CYCLED_GRAPH);
                nodesWalked[node] = true;
                var nextNodes = coherences.GetCoheredFrom(node).Select(c => c.To);
                foreach (var next in nextNodes)
                {
                    walkFrom(next);
                }
            }
            IEnumerable<Node> unwalked;
            IEnumerable<Node> getUnwalked() => nodesWalked.Where(kv => kv.Value == false).Select(kv => kv.Key);

            for (unwalked = getUnwalked(); unwalked.Any(); unwalked = getUnwalked())
                try
                {
                    walkFrom(unwalked.First());
                }
                catch (Exception e)
                {
                    if (e.Message == GraphExceptions.CYCLED_GRAPH)
                        return true;
                    else
                        throw e;
                }
            return false;

        }
    }
}
