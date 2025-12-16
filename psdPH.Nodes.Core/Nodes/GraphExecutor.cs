using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.Core.Nodes
{
    public class GraphExecutor
    {
        static void Execute(NodeGraph graph, Node node,DocumentWr doc)
        {
            IEnumerable<NodeLetLink> RelatedLinks(Node n) => graph.NodeLetLinks.Where(link => link.From.Node == n);
            node.Execute(doc);
            foreach (var link in RelatedLinks(node))
            {
                link.To.Let.Value = link.From.Let.Value;
                Execute(graph, link.To.Node, doc);
            }
        }
        public static void Execute(NodeGraph graph,DocumentWr doc)
        {
            Execute(graph, graph.RootNode,doc);
        }
    }
}
