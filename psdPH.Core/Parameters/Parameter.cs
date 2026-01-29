using psdPH.Logic;
using psdPH.Setups;
using psdPH.Utils;
using System;
using System.Collections.Generic;

namespace psdPH.Parameters
{
    public abstract class Parameter
    {
        public object Value;
        public string Name;
        public Parameter(string name):this()
        {
            Name = name;
        }
        public Parameter():base() {
        }
        public override string ToString()=>Name;
        public virtual void Import(Parameter parameter)
        {
            Name = parameter.Name;
            Value = parameter.Value;
        }
        public virtual Parameter Clone()
        {
            Parameter result = Activator.CreateInstance(GetType()) as Parameter;
            result.Import(this);
            return result;
        }
    }
}
