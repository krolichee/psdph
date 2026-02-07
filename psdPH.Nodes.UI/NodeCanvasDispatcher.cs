using System;

namespace psdPH.Nodes.UI
{
    public class NodeCanvasDispatcher
    {
        NodeGraph NodeGraph;
        NodeLet sourceLet;

        private NodeCanvasDispatcher()
        {
        }

        public NodeCanvasDispatcher(NodeGraph nodeGraph)
        {
            NodeGraph = nodeGraph;
        }

        public NodeLet SourceLet { get => sourceLet; set => sourceLet = value; }

        public void DeleteLink(NodeLetLink link)
        {
            NodeGraph.DeleteLink(link);
        }

        public void DeleteNode(Node node)
        {
            NodeGraph.DeleteNode(node);
        }

        public void PullLinkTo(NodeLet nodeLet)
        {
            //TODO добавить обработку отсуствия source
            NodeGraph.LetLink(sourceLet, nodeLet);
        }
    }
}