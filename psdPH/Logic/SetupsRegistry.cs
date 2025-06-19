using System;
using System.Collections.Generic;

namespace psdPH.Logic.Compositions
{
    public class SetupsRegistry
    {
        public static Dictionary<Type, SetupsSource> Instances = new Dictionary<Type, SetupsSource>();
        public static void Register<T>(SetupsSource source) => Register(typeof(T), source);
        public static void Register(Type type, SetupsSource source) =>
            Instances[type] = source;
        public static SetupsSource GetFor(object obj)=> Instances[obj.GetType()];
    }

}

