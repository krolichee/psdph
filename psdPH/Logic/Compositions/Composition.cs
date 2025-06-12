using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    [PsdPhSerializable]
    public abstract class Composition : ISetupable, psdPH.ISerializable
    {
        public ParameterSet ParameterSet = new ParameterSet();
        public delegate void RulesetUpdated();
        public delegate void ChildrenUpdated();
        public event ChildrenUpdated ChildrenUpdatedEvent;
        public event RulesetUpdated RulesetUpdatedEvent;
        public string UIName
        {
            get
            {
                Type type = this.GetType();
                UINameAttribute rootAttribute = (UINameAttribute)Attribute.GetCustomAttribute(type, typeof(UINameAttribute));
                return rootAttribute.PositionalString;
            }
        }
        public List<Composition> Children = new List<Composition>();
        internal void AddChildren(Composition[] compositions)
        {
            foreach (var item in compositions)
            {
                AddChild(item);
            }
        }
        abstract public string ObjName { get; }
        public override string ToString()
        {
            return ObjName;
        }
        [XmlIgnore]
        virtual public Composition Parent { get; set; }
        protected T[] Siblings<T>() where T:Composition
        {
            if (Parent == null)
                return new Composition[0] as T[];
            return Parent.GetChildren<T>().ToArray();
        }
        [XmlIgnore]
        public abstract Setup[] Setups { get; }

        abstract public void Apply(Document doc);
        protected void invokeChildrenEvent()
        {
            ChildrenUpdatedEvent?.Invoke();
        }
        virtual public void AddChild(Composition child) { }
        virtual public void RemoveChild(Composition child) { }
        public void Restore(Composition parent = null)
        {
            RestoreParents(parent);
            RuleSet.RestoreComposition(this);
        }
        virtual public void RestoreParents(Composition parent = null)
        {
            if (parent != null)
                Parent = parent;
            if (GetChildren() != null)
                foreach (var item in GetChildren())
                    item.Restore(this);
        }
        virtual public Composition[] GetChildren() { return null; }
        virtual public T[] GetChildren<T>() { return null; }
        [Obsolete]
        public bool IsSetUp() => true;

        public RuleSet RuleSet = new RuleSet();

        public Composition() { 
            ChildrenUpdatedEvent += () => Restore(); 
            RuleSet.Updated += () => RulesetUpdatedEvent?.Invoke(); 
            this.AddTypeToKnownTypes(); 
        }
        public abstract bool IsMatching(Document doc);
        public virtual MatchingResult IsMatchingRouted(Document doc)
        {
            return new MatchingResult(this,IsMatching(doc));
        }
    }
    
    
}

