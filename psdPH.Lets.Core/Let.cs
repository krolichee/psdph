using psdPH.Setups;
using System;
using System.Xml.Linq;

namespace psdPH.Lets
{
    public class Let
    {
        readonly ReflectionConfig config;
        readonly Func<string> nameGetter;
        public Let(ReflectionConfig config)
        {
            this.config = config;
            nameGetter = () => config.FieldName;
        }
        public Let(ReflectionConfig config, Func<string> nameGetter) : this(config)
        {
            this.nameGetter = nameGetter;
        }
        public Let(ReflectionConfig config, string name) : this(config)
        {
            this.nameGetter = ()=>name;
        }

        public object Value { get => config.GetValue(); set => config.SetValue(value); }
        public string Name => nameGetter();
        
    }
}
