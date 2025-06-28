using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Serialization;
using psdPH.Nodes;
using psdPH.Parameters;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    public abstract class Composition : DtoGuided
    {

        //ParameterSet
        public ParameterSet ParameterSet = new ParameterSet();

        //Hierarchy
        public delegate void ChildrenUpdated();
        public event ChildrenUpdated ChildrenUpdatedEvent;
        public List<Composition> Children = new List<Composition>();
        public void AddChildren(Composition[] compositions)
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
        abstract public void Apply(DocumentWr doc);
        public abstract bool IsMatching(DocumentWr doc);
        public virtual MatchingResult IsMatchingRouted(DocumentWr doc)
        {
            return new MatchingResult(this, IsMatching(doc));
        }
        protected void matchChildren(MatchingResult result, DocumentWr doc)
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
        public Composition():base() { 
            ChildrenUpdatedEvent += () => Restore(); 
        }
        
    }
    
    
}

