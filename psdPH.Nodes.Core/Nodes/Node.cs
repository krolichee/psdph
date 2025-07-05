using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;
using System.Collections.ObjectModel;

namespace psdPH.Nodes
{
    public abstract class Node:DtoGuided,ISerializable
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
            public bool Cycled { get
                {
                    bool check(Node node1,Node node2)
                    {
                        if (node1 == node2)
                            return true;
                        foreach (NodeSetupLink nsl in node1.Links)
                        {
                            if (check(nsl.ToNodeSetup.Node,node2))
                                return true;
                        }
                        return false;
                    }
                    return check(ToNodeSetup.Node,FromNodeSetup.Node);
                } 
            }
        }
        [XmlIgnore]
        Dictionary<Node, bool> ParentAppliedDict = new Dictionary<Node, bool>();
        protected event Action<Node> Applied;
        [XmlIgnore]
        public abstract List<Setup> Inputs { get; }
        [XmlIgnore]
        public abstract List<Setup> Outputs { get; }
        [XmlIgnore]
        public Setup[] IOSetups => Inputs.Concat(Outputs).ToArray();
        [XmlIgnore]
        public ObservableCollection<NodeSetupLink> Links = new ObservableCollection<NodeSetupLink>();

        protected Node():base()
        {
        }
        public void Apply()
        {
            _apply();
            foreach (var item in Links)
            {
                var outputNodeSetup = item.FromNodeSetup;
                var otherNodeSetup = item.ToNodeSetup;

                var otherNode = otherNodeSetup.Node;
                var otherSetup = otherNode.Inputs.First(s=>
                s.Equals(otherNodeSetup.Setup));
                var outputValue = outputNodeSetup.Setup.Config.GetValue();
                if (!otherSetup.IsValidValue(outputValue))
                    throw new NotCompatibleSetupException();
                otherSetup.Config.SetValue(outputValue);
            }
            Applied?.Invoke(this);
        }
        protected abstract void _apply();
        
        public virtual bool CheckLink(Setup inSetup, Setup outSetup) => outSetup.MayImport(inSetup);
        public void Link(Setup thisSetup, Node other,Setup otherSetup)
        {
            if (!CheckLink(thisSetup, otherSetup))
                throw new NotCompatibleSetupException();
            if (other.IsLinkedSetup(otherSetup))
                throw new NotCompatibleSetupException();
            if (other == this)
                throw new NotCompatibleSetupException();
            var link = new NodeSetupLink(new NodeSetup(this, thisSetup), new NodeSetup(other, otherSetup));
            if (link.Cycled)
                throw new NotCompatibleSetupException();
            Links.Add(link);
            other.Subscribe(this);
        }
        private void ParentApplied(Node node)
        {
            ParentAppliedDict[node] = true;
            if (ParentAppliedDict.All(p => p.Value))
                Apply();
        }

        void Subscribe(Node node)
        {
            if(!ParentAppliedDict.TryGetValue(node,out var _))
                ParentAppliedDict.Add(node,false);
            node.Applied += ParentApplied;
        }
        public NodeSetupLink[] GetOutputLinksToNodeSetup(NodeSetup nodeSetup)
        {
            var outputLinksToNode = getOutputLinksToNode(nodeSetup.Node);
            return outputLinksToNode.Where(ol => ol.ToNodeSetup.Equals(nodeSetup.Setup)).ToArray() ;
        }
        public NodeSetupLink[] getOutputLinksToNode(Node node)
        {
            return Links.Where(ol => ol.ToNodeSetup.Node == node).ToArray();
        }
        Setup[] LinkedSetups { get
            {
                List<NodeSetupLink> outputLinksToThis = new List<NodeSetupLink>();
                foreach (var item in ParentAppliedDict.Select(kv => kv.Key))
                {
                    outputLinksToThis.AddRange(item.getOutputLinksToNode(this));
                }
                var thisLinkedSetups = outputLinksToThis.Select(ol => ol.ToNodeSetup.Setup);
                return thisLinkedSetups.ToArray();
            } 
        }
        public bool IsLinkedSetup(Setup thisSetup)
        {
            return LinkedSetups.Contains(thisSetup);
        }

        public void Unlink(NodeSetup from, NodeSetup to)
        {
            Links.Remove(Links.First(l=>l.FromNodeSetup.Equals(from) && l.ToNodeSetup.Equals(to)));
            to.Node.Unsubscribe(this);
        }
        public void Unlink(Node other)
        {
            other.Unsubscribe(this);
        }
        void Unsubscribe(Node node)
        {
            if (!ParentAppliedDict.TryGetValue(node, out var _))
                ParentAppliedDict.Remove(node);
            node.Applied -= ParentApplied;
        }
    }
}
