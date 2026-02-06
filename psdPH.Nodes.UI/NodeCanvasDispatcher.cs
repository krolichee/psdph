using System;

namespace psdPH.Nodes.UI
{
    internal class NodeCanvasDispatcher
    {
        NodeGraph NodeGraph;
        NodeLet sourceLet;
        public static NodeCanvasDispatcher Instance { get; internal set; }

        internal void DeleteLink(NodeLet from, NodeLet to)
        {
            NodeGraph.
        }

        internal void DeleteNode(Node node)
        {
            NodeGraph.DeleteNode(node);
        }

        internal void PullLinkTo(NodeLet nodeLet)
        {
            //TODO добавить обработку отсуствия source
            NodeGraph.LetLink(sourceLet, nodeLet);
        }
    }
}