using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public static class NodesOutputFieldnamesRegistry
    {
        public static Dictionary<Type, string> Instances = new Dictionary<Type, string>();
        public static void Register<T>(string name) => Register(typeof(T), name);
        public static void Register(Type type, string name) =>
            Instances[type] = name;
        public static string GetFor(object obj) => Instances[obj.GetType()];
    }
}
