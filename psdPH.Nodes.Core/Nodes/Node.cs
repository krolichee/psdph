using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using psdPH.Setups;
using System.Collections.ObjectModel;
using psdPH.Photoshop;

namespace psdPH.Nodes
{
    public class Let
    {
        public object Obj => obj;

        public Type Type => type;
        public object Value { get => get(); set => set(value); }

        Func<object> get;
        Action<object> set;

        object obj;
        private Type type;
        string name;

        public Let(object obj, string name, Type type,Func<object> get,Action<object> set)
        {
            this.name = name;
            this.type = type;
            this.get = get;
            this.set = set;
            this.obj = obj;
        }
    }
    abstract public class Node
    {
        public abstract Let[] Inlets { get; }
        public abstract Let[] Outlets { get; }
        public abstract Let[] Chain { get; }
        public abstract void Execute(DocumentWr doc);
    }
}
