using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
    class GraphExecution
    {
        List<QueueCoherence> QueueCoherences;
        Dictionary<Node, bool> NodesExecuted;

        public GraphExecution(NodeGraph graph)
        {
            QueueCoherences = ExtractCoherences(graph).Select(c=>new QueueCoherence(c.From,c.To)).ToList();
            NodesExecuted = graph.Nodes.ToDictionary(n => n, _ => false);
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
        public Node[] GetReadyNodes(NodeGraph graph)
        {
            return NonExecutedNodes().Where(n => IsAllCoherencesExecuted(n) && IsAllChainsLet(n, graph)).ToArray();
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

        IEnumerable <Node> NonExecutedNodes() => NodesExecuted.Where(kv => !kv.Value).Select(kv => kv.Key);
        
        IEnumerable<ChainLink> GetIngoingChains(Node node, NodeGraph graph) => graph.ChainLinks.Where(cl => cl.ToNode == node);
        private bool IsAllChainsLet(Node node, NodeGraph graph)
        {
            return GetIngoingChains(node, graph).Select(cl => cl.FromLet.Let.Value).All(b=>(bool)b);
        }
    }
    public static class GraphExecutor
    {
        //TODO Сделать не статическим
        static GraphExecution execution;
        static void Execute(NodeGraph graph, Node node,DocumentWr doc)
        {
            //Если есть chains:
            //По приоритету:
            //1. Ожидать выполнения links
            //2. Ожидать выполнения предшествующей ноды чейна

            //Если только Links:
            //По приоритету:
            //1. Ожидать выполнения links

            /*
             * 0. Начать с Root
             * 1. Выполнить текущую ноду
             * 2. Отметить все сцепления, которые ведут от неё
             * 3. Найти ноды со всеми выполнеными сцеплениями
             * 4. Для каждой этой ноды, повторить 1-4
             */

            //Исполнить входящие линки
            PullLets(graph,node);
            node.Execute(doc);
            execution.TickExecuted(node);
            var readyNodes = execution.GetReadyNodes(graph);
            foreach (var next_node in readyNodes)
            {
                Execute(graph, next_node, doc);
            }
            
        }

        private static void PullLets(NodeGraph graph, Node node)
        {
            foreach (var nodeLetLink in GetInboundLetLinks(graph, node))
            {
                nodeLetLink.Push();
            }
        }

        private static IEnumerable<NodeLetLink> GetInboundLetLinks(NodeGraph graph, Node node)
        {
            return graph.NodeLetLinks.Where(nll => nll.To.Node == node);
        }

        public static void Execute(NodeGraph graph,DocumentWr doc)
        {
            
            if (graph.RootNode == null)
                throw new ArgumentException("Root node is null");
            execution = new GraphExecution(graph);
            ///TODO Если есть "висящие" исполняемые ноды: Ошибка
            Execute(graph, graph.RootNode,doc);
        }
    }
}
