using System.Collections.Generic;

namespace psdPH.Nodes
{
    public abstract partial class Node
    {
        public class NodeSetupLink
        {
            public NodeSetup FromNodeSetup;
            public NodeSetup ToNodeSetup;
            public NodeSetupLink(NodeSetup from, NodeSetup to)
            {
                FromNodeSetup = from;
                ToNodeSetup = to;
            }
            public bool Cycled
            {
                get
                {
                    bool check(Node node1, Node node2)
                    {
                        if (node1 == node2)
                            return true;
                        foreach (var nsl in node1.ParentAppliedDict)
                        {
                            if (check(nsl.Key, node2))
                                return true;
                        }
                        return false;
                    }
                    return check(ToNodeSetup.Node, FromNodeSetup.Node);
                }
            }
            public override bool Equals(object obj)
            {
                if (obj is NodeSetupLink other)
                    return other.FromNodeSetup.Equals(FromNodeSetup) && other.ToNodeSetup.Equals(ToNodeSetup);
                return false;
            }

            public override int GetHashCode()
            {
                return FromNodeSetup.GetHashCode() * 23 + ToNodeSetup.GetHashCode();
            }
        }
    }
}
