using psdPH.Logic;
using psdPH.Logic.Serialization;
using psdPH.Nodes;
using psdPH.Setups;
using psdPH.Utils;
using System;
using System.Collections.Generic;

namespace psdPH.Parameters
{
    public abstract class Parameter: Guided, ISetupable
    {
        public object Value;
        public string Name;

        public event SetupsChangedEvent SetupsChanged;

        public abstract Setup[] Setups { get; }


        protected ReflectionConfig getValueSetupConfig() => new ReflectionConfig(this, nameof(Value), "Значение");
        public bool IsSetUp()=>Name != null;
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
