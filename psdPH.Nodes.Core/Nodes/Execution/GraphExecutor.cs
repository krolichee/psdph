using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.Core.Nodes
{
    public class GraphExecutor
    {
        //TODO Сделать не статическим
        GraphExecution Execution;
        NodeGraph Graph;
        void Execute(Node node,DocumentWr doc)
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
            PullLets(node);
            node.Execute(doc);
            Execution.TickExecuted(node);
            var readyNodes = Execution.GetReadyNodes();
            foreach (var next_node in readyNodes)
            {
                Execute(next_node, doc);
            }
            
        }

        private void PullLets(Node node)
        {
            foreach (var nodeLetLink in GetInboundLetLinks(node))
            {
                nodeLetLink.Push();
            }
        }

        private IEnumerable<NodeLetLink> GetInboundLetLinks(Node node)
        {
            return Graph.NodeLetLinks.Where(nll => nll.To.Node == node);
        }

        public static void Execute(NodeGraph graph,DocumentWr doc)
        {
            
            if (graph.RootNode == null)
                throw new ArgumentException("Root node is null");
            var executor = new GraphExecutor(graph);
            ///TODO Если есть "висящие" исполняемые ноды: Ошибка
            executor.Execute(graph.RootNode,doc);
        }
        private GraphExecutor(NodeGraph graph)
        {
            Execution = new GraphExecution(graph);
            Graph = graph;
        }
    }
}
