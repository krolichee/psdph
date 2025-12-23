using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Serialization;
using psdPH.Photoshop;
using System;
using System.Linq;
using System.Xml.Serialization;
using psdPH.Core.Compositons.Compositions;
using psdPH.Nodes;

namespace psdPH
{
    /// <summary>
    /// Представляет собой элемент структуры документа
    /// </summary>
    public abstract class Composition : IHierarchial<Composition>,IDocumentMatchable,INodable
    {

        Hierarchy<Composition> hierarchy;
        string name;
        public Composition Clone()
        {
            Composition result = Cloner.Clone(this) as Composition;
            result.Hierarchy.Restore(this);
            return result;
        }
        public Composition()
        {
            hierarchy = new Hierarchy<Composition>(this);
        }
        public Hierarchy<Composition> Hierarchy { get => hierarchy; }
        public virtual string Name { get => name; set => name = value; }
        public abstract Let[] Chain { get; }
        public abstract Let[] Inlets { get; }
        public abstract Let[] Outlets { get; }

        public abstract void Apply(DocumentWr doc);
        public abstract bool IsMatching(DocumentWr doc);
        public virtual MatchingResult IsMatchingRouted(DocumentWr doc) => new MatchingResult(this, IsMatching(doc));
        public override string ToString() => Name;
        public void Execute()=>Apply(PhotoshopWrapper.GetActiveDocument());
        internal void SetHierarchy(Hierarchy<Composition> hierarchy) => this.hierarchy = hierarchy;
    }
    
    
}

