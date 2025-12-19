using psdPH.Nodes.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public static class GraphAnalyzer
    {
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
            return graph.Nodes.Where(n=>n is SourceNode);
        }
        public static Node GetRootNode(this NodeGraph graph)
        {
            var rootNodes = GetRootishNodes(graph).Except(GetSourceNodes(graph));
            if (rootNodes.Count() > 1)
                throw new ArgumentException("Graph has multiple root nodes");
            return rootNodes.First();
        }
        public static IEnumerable<Node> GetRootishNodes(this NodeGraph graph)
        {
            return GetCoherences(graph).Select(c => c.To).Distinct();
        }
        public static bool IsCycled()
        {

        }
    }
}
