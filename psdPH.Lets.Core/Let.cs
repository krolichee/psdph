using psdPH.Lets.Core;
using psdPH.Reflection;
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
        public Type Type => config.GetTypeOfMember();
        public object Obj => config.Obj;

        public static Let FromField(object obj, string fieldName)
        {
            var config = new ReflectionConfig(obj,fieldName);
            return new Let(config);
        }
    }
}
