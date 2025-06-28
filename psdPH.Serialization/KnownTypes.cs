using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
   public interface ISerializable { }
    public static class KnownTypes
    {
        static KnownTypes()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(ISerializable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            Types.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException) { }
            }
        }
        public static HashSet<Type> Types = new HashSet<Type>();
        //public static void AddTypeToKnownTypes(this object obj)
        //{
        //    Types.Add(obj.GetType());
        //}
    }
}
