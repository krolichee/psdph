using System;

namespace psdPH.Nodes
{
    //TODO наследовать от Coherence
    public class NodeLetLink
    {
        public NodeLet From { get; private set; }
        public NodeLet To { get; private set; }

        public NodeLetLink(NodeLet from, NodeLet to)
        {
            From = from;
            To = to;
        }

        public void Push()
        {
            To.Let.Value = From.Let.Value;
        }

        public bool IsChain()
        {
            return !From.IsFlowlet() && To.IsFlowlet() ;
        }
        public override bool Equals(object obj)
        {
            if (obj is NodeLetLink other)
                return other.From.Equals(From) && other.To.Equals(To);
            return false;
        }
    }
}
