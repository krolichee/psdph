using System;
using System.Collections.Generic;

namespace psdPH.Logic.Compositions
{
    public class DtoConvertersRegistry
    {
        private static readonly Dictionary<Type, DtoConverter> _converters = new Dictionary<Type, DtoConverter>();

        public static void Register<T>(DtoConverter converter)
        {
            Register(typeof(T), converter);
        }
        public static void Register(Type type, DtoConverter converter)
        {
            _converters[type] = converter;
        }
        public static DtoConverter Get<T>()
        {
            return _converters[typeof(T)];
        }
        public static DtoConverter GetFor(object obj)
        {
           var type = obj.GetType();
           return _converters[type];
        }
    }

}

