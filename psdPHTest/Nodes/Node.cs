using System;
using System.Linq;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using psdPH.Nodes;

namespace psdPH.Nodes
{
    public abstract class Node:Guided
    {
        Dictionary<Node, bool> ParentAppliedDict = new Dictionary<Node, bool>();
        protected event Action<Node> Applied;
        public abstract List<Setup> Inputs { get; }
        public abstract List<Setup> Outputs { get; }
        public Dictionary<Setup, Setup> OutputLinks = new Dictionary<Setup, Setup>();
        public void Apply()
        {
            _apply();
            foreach (var item in OutputLinks)
            {
                var outputSetup = item.Key;
                var otherSetup = item.Value;
                var outputValue = outputSetup.Config.GetValue();
                if (!otherSetup.IsValidValue(outputValue))
                    throw new NotCompatibleSetupException();
                otherSetup.Config.SetValue(outputValue);
            }
            Applied?.Invoke(this);
        }
        protected abstract void _apply();
        public Guid Guid { get; set; }
        bool isCompatibleSetups(Setup inSetup,Setup outSetup)
        {
            return inSetup.Config.GetFieldOrPropertyType().IsSubclassOf(outSetup.Config.GetFieldOrPropertyType());
        }
        protected virtual bool checkLink(Setup inSetup, Setup outSetup) => isCompatibleSetups(inSetup,outSetup);
        public void Link(Setup thisSetup, Node other,Setup otherSetup)
        {
            OutputLinks.Add(thisSetup, otherSetup);
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
