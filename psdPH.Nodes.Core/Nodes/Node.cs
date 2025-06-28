using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;

namespace psdPH.Nodes
{
    public abstract class Node:Guided,ISerializable
    {
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
        public Dictionary<Setup, NodeSetupLink> OutputLinks = new Dictionary<Setup, NodeSetupLink>();

        protected Node():base()
        {
        }
        public void Apply()
        {
            _apply();
            foreach (var item in OutputLinks)
            {
                var outputSetup = item.Key;
                var otherNodeSetup = item.Value;

                var otherNode = otherNodeSetup.Node;
                var otherSetup = otherNode.Inputs.First(s=>
                s.Equals(otherNodeSetup.Setup));
                var outputValue = outputSetup.Config.GetValue();
                if (!otherSetup.IsValidValue(outputValue))
                    throw new NotCompatibleSetupException();
                otherSetup.Config.SetValue(outputValue);
            }
            Applied?.Invoke(this);
        }
        protected abstract void _apply();
        
        protected virtual bool checkLink(Setup inSetup, Setup outSetup) => true;
        public void Link(Setup thisSetup, Node other,Setup otherSetup)
        {
            if (!checkLink(thisSetup, otherSetup))
                throw new NotCompatibleSetupException();
            OutputLinks.Add(thisSetup,new NodeSetupLink(other,otherSetup));
            other.Subscribe(this);
        }
        private void ParentApplied(Node node)
        {
            ParentAppliedDict[node] = true;
            if (ParentAppliedDict.All(p => p.Value))
                Apply();
        }

        public void Subscribe(Node node)
        {
            ParentAppliedDict.Add(node,false);
            node.Applied += ParentApplied;
        }
        
    }
}
