using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    public static class DtoRegistry
    {
        public static HashSet<Type> Types = new HashSet<Type>();
        public static void Add(Type type)
        {
            Types.Add(type);
        }
        public static Type[] GetTypes => Types.ToArray();
    }
}
