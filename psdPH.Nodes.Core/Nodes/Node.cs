using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;
using System.Collections.ObjectModel;

namespace psdPH.Nodes
{
    public delegate void NodeEvent(Node node);
    public abstract partial class Node:DtoGuided,ISerializable
    {
        [XmlIgnore]
        Dictionary<Node, bool> ParentAppliedDict = new Dictionary<Node, bool>();
        protected event Action<Node> Applied;
        [XmlIgnore]
        public virtual Setup[] Chains => new Setup[0];
        [XmlIgnore]
        public abstract List<Setup> Inputs { get; }
        [XmlIgnore]
        public abstract List<Setup> Outputs { get; }
        [XmlIgnore]
        public Setup[] IOSetups => Inputs.Concat(Outputs).ToArray();
        [XmlIgnore]
        public ObservableCollection<NodeSetupLink> Links = new ObservableCollection<NodeSetupLink>();
        NodeSetup _chain;
        [XmlIgnore]
        public NodeSetup Chain
        {
            get => _chain;
            set
            {
                _chain = value;
                ChainChanged?.Invoke();
                
            }
        }


        public event Action ChainChanged;
        protected Node():base() { }
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
        
        public virtual bool CheckOutLink(Setup thisSetup, Setup otherSetup) =>
            otherSetup.MayImport(thisSetup);
        public static void Link(NodeSetup from, NodeSetup to)
        {
            from.Node.LinkOut(from.Setup,to.Node,to.Setup);
        }
        public void LinkOut(Setup thisSetup, Node other,Setup otherSetup)
        {
            if (!CheckOutLink(thisSetup, otherSetup))
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
        public void ChainIn(Node node)
        {
            
            Subscribe(node);
        }
        public void ChainIn(NodeSetup chain)
        {
            if (Chain!=null)
                throw new NotCompatibleSetupException();
            if(new NodeSetupLink( new NodeSetup(this, Setup.None), new NodeSetup(chain.Node, Setup.None)).Cycled)
                throw new NotCompatibleSetupException();
            ChainIn(chain.Node);
            Chain = chain;
            
        }
        public void Unchain(NodeSetup chain)
        {
            Chain = null;
            Unsubscribe(chain.Node);
        }
        bool ChainAllows { get {
                if (Chain?.Setup.IsNone() != false)
                    return true;
                bool? chainResult = (bool?)Chain?.Setup.Config.GetValue();
                return chainResult == true; 
            } 
        }
        bool AllParentsApplied => ParentAppliedDict.All(p => p.Value);
        private void ParentApplied(Node node)
        {
            ParentAppliedDict[node] = true;
            if (AllParentsApplied)
                if (ChainAllows)
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

        public static void Unlink(NodeSetup from, NodeSetup to)
        {
            var fromNode = from.Node;
            fromNode.Links.Remove(fromNode.Links.First(l=>l.ToNodeSetup.Equals(to)));
            to.Node.Unsubscribe(fromNode);
        }
        public void Unlink(Node other)
        {
            other.Unsubscribe(this);
        }
        void Unsubscribe(Node node)
        {
            if (ParentAppliedDict.TryGetValue(node, out var _))
                ParentAppliedDict.Remove(node);
            node.Applied -= ParentApplied;
        }
    }
}
