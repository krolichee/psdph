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
        public object Obj => value;

        public Type Type { get; internal set; }
        public object Value { get; internal set; }

        private object value;
        private string v;
        private Type type;

        public Let(object value, string v, Type type)
        {
            this.value = value;
            this.v = v;
            this.type = type;
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
