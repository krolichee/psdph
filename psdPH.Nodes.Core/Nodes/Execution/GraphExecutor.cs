using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace psdPH.Nodes.Core.Nodes
{
    public class GraphExecutor
    {
        readonly GraphExecution Execution;
        readonly NodeGraph Graph;
        private GraphExecutor(NodeGraph graph)
        {
            Execution = new GraphExecution(graph);
            Graph = graph;
        }
        
        void ExecuteNode(Node node)
        {
            PullLets(node);
            node.Execute();
            Execution.TickExecuted(node);
        }
        void Execute()
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

            foreach (var node in Graph.GetSourceNodes())
            {
                ExecuteNode(node);
            }

            var rootNode = Graph.GetRootNode();

            
            ExecuteNode(rootNode);

            IEnumerable<Node> readyNodes = Execution.GetReadyNodes();
            for (; 
                readyNodes.Count() > 0; 
                readyNodes = Execution.GetReadyNodes())
            {
                ExecuteNode(readyNodes.First());
            }
            
        }

        void PullLets(Node node)
        {
            foreach (var nodeLetLink in GetInboundLetLinks(node))
            {
                nodeLetLink.Push();
            }
        }

        IEnumerable<NodeLetLink> GetInboundLetLinks(Node node)
        {
            return Graph.NodeLetLinks.Where(nll => nll.To.Node == node);
        }
        public static void ExecuteGraph(NodeGraph graph)
        {
            CheckGraph(graph);
            var executor = new GraphExecutor(graph);
            executor.Execute();
        }

        private static void CheckGraph(NodeGraph graph)
        {
            graph.GetRootNode();
            if (graph.IsCycled())
                throw new ArgumentException();

        }
    }
}
