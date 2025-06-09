

using System.Collections.Generic;

namespace psdPH.Logic.Parameters
{
    public abstract class Parameter:ISerializable,ISetupable
    {
        public object Value;
        public string Name;
        public abstract Setup[] Setups { get; }

        protected SetupConfig getValueSetupConfig() => new SetupConfig(this, nameof(Value), Name);
        public bool IsSetUp()=>Name != null;
        public Parameter(string name)
        {
            Name = name;
        }
        public override string ToString()=>Name;
        
    }
}
