using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Serialization
{
    //TODO преобразовать в одиночку
    public class DtoConvertersRegistry
    {
        static HashSet<DtoConverter> converters = new HashSet<DtoConverter>();
        public static void Register(DtoConverter converter)
        {
            converters.Add(converter);
        }
        public static DtoConverter GetForEntity(object entity)
        {
            return GetForEntityType(entity.GetType());
        }
        public static DtoConverter GetForEntityType(Type type)
        {
            return converters.First(c=>c.EntityType == type);
        }
        public static DtoConverter GetForDto(Dto dto)
        {
            return GetForDtoType(dto.GetType());
        }
        public static DtoConverter GetForDtoType(Type type)
        {
            return converters.First(c => c.DtoType == type);
        }
    }

}

