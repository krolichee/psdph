using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Utils;
using psdPH.Utils.Setups;
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
    public abstract class Composition : ISerializable
    {
        //DTO
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).ApplyDto(this, value);
        }
        public Guid Guid;

        //ParameterSet
        public ParameterSet ParameterSet = new ParameterSet();

        //RuleSet
        public delegate void RulesetUpdated();
        public event RulesetUpdated RulesetUpdatedEvent;
        public RuleSet RuleSet = new RuleSet();

        //Hierarchy
        public delegate void ChildrenUpdated();
        public event ChildrenUpdated ChildrenUpdatedEvent;
        public List<Composition> Children = new List<Composition>();
        internal void AddChildren(Composition[] compositions)
        {
            foreach (var item in compositions)
            {
                AddChild(item);
            }
        }
        [XmlIgnore]
        virtual public Composition Parent { get; set; }
        protected T[] Siblings<T>() where T : Composition
        {
            if (Parent == null)
                return new Composition[0] as T[];
            return Parent.GetChildren<T>().ToArray();
        }
        protected void invokeChildrenUpdatedEvent()
        {
            ChildrenUpdatedEvent?.Invoke();
        }
        public void AddChild(Composition child)
        {
            child.Parent = this;
            Children.Add(child);
            invokeChildrenUpdatedEvent();
        }
        public void RemoveChild(Composition child)
        {
            Children.Remove(child);
            invokeChildrenUpdatedEvent();
        }
        public Composition[] GetChildren() =>
           Children.ToArray();

        public T[] GetChildren<T>() =>
            Children.Where(l => l is T).Cast<T>().ToArray();
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

        //String represetations
        public string UIName
        {
            get
            {
                Type type = this.GetType();
                UINameAttribute rootAttribute = (UINameAttribute)Attribute.GetCustomAttribute(type, typeof(UINameAttribute));
                return rootAttribute.PositionalString;
            }
        }
        abstract public string ObjName { get; }
        public override string ToString()
        {
            return $"[{UIName}] {ObjName}";
        }

        //Using
        abstract public void Apply(Document doc);
        public abstract bool IsMatching(Document doc);
        public virtual MatchingResult IsMatchingRouted(Document doc)
        {
            return new MatchingResult(this, IsMatching(doc));
        }
        protected void matchChildren(MatchingResult result, Document doc)
        {
            foreach (var child in Children)
            {
                var r = child.IsMatchingRouted(doc);
                result.Match &= r;

                if (!result)
                {
                    result.MismatchRoute.AddRange(r.MismatchRoute);
                    break;
                }
            }
        }
        public Composition Clone()
        {
            Composition result = CloneConverter.Clone(this) as Composition;
            result.Restore(Parent);
            return result;
        }

        //Constructors
        public Composition() { 
            ChildrenUpdatedEvent += () => Restore(); 
            RuleSet.Updated += () => RulesetUpdatedEvent?.Invoke(); 
        }
        
    }
    
    
}

