using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Core.Compositons.Compositions
{
    public interface IHierarchial<T> where T : IHierarchial<T>
    {
        Hierarchy<T> Hierarchy { get; }
    }
    public class Hierarchy<TIH>where TIH:IHierarchial<TIH>
    {
        TIH self;
        TIH parent = default;
        List<TIH> children = new List<TIH>();

        public Hierarchy(TIH self)
        {
            this.self = self;
            ChildrenUpdatedEvent += ()=>Restore();
        }

        public delegate void ChildrenUpdated();
        public event ChildrenUpdated ChildrenUpdatedEvent;
        public IEnumerable<TIH> Children => children;
        public TIH Self => self;
        public void AddChildren(TIH[] compositions)
        {
            foreach (var item in compositions)
            {
                AddChild(item);
            }
        }
        public TIH Parent { get => parent; set => parent = value; }
        public T[] GetSiblings<T>() where T : TIH
        {
            if (parent == null)
                return new TIH[0] as T[];
            return parent.Hierarchy.GetChildren<T>().ToArray();
        }
        protected void InvokeChildrenUpdatedEvent()
        {
            ChildrenUpdatedEvent?.Invoke();
        }
        public void AddChild(TIH child)
        {
            child.Hierarchy.Parent = self;
            children.Add(child);
            InvokeChildrenUpdatedEvent();
        }
        public void RemoveChild(TIH child)
        {
            children.Remove(child);
            InvokeChildrenUpdatedEvent();
        }

        public T[] GetChildren<T>() =>
            Children.Where(l => l is T).Cast<T>().ToArray();
        public void Restore(TIH parent = default)
        {
            RestoreParents(parent);
        }
        virtual public void RestoreParents(TIH parent = default)
        {
            if (parent != null)
                Parent = parent;
            foreach (var item in Children)
                    item.Hierarchy.Restore(self);
        }
    }
}
