using System;

namespace psdPH.Nodes
{
    public class Let
    {
        public object Obj { get; private set; }

        public Type Type { get; private set; }
        public object Value { get => get(); set => set(value); }

        Func<object> get;
        Action<object> set;
        public string Name { get; private set; }

        public Let(object obj, string name, Type type,Func<object> get,Action<object> set)
        {
            this.Name = name;
            this.Type = type;
            this.get = get;
            this.set = set;
            this.Obj = obj;
        }
    }
}
