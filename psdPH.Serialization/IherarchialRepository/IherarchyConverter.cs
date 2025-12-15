using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    static class IherarchyConverter
    {
        static DtoScope getRelatedDtoScope(object entity, ConversionContext context)
        {
            //Создание возвращаемого dtoScope
            var dtoScope = new DtoScope();
            //Поиск конвертера для корневого объекта
            var converter = DtoConvertersRegistry.GetForEntity(entity);
            //Получение DTO
            var entityDto = converter.GetDto(entity, out UnknownGuidReference[] pRefs);
            context.PendingReferences.AddRange(pRefs);
            context.IdentityMap.AddMapping(entity,entityDto.Guid);
            //Разрешение ссылок уже существующими объектами включая текущий
            ResolveReferences(context);
            //Пополнение возвращаемого dtoScope DTO корневого объекта
            dtoScope.Scope.Add(entityDto);
            //Добавление в dtoScope объектов из неразрешённых ссылок
            foreach (object obj in pRefs.Select(pR => pR.TargetEntity))
            {
                var dScope = getRelatedDtoScope(obj, context);
                dtoScope.Scope.AddRange(dScope.Scope);
            }
            return dtoScope;
        }
        static bool ResolveReferences(ConversionContext context)
        {
            bool full = true;
            foreach (var pRef in context.PendingReferences.ToList())
                if (context.IdentityMap.TryGetId(pRef.TargetEntity, out Guid id))
                {
                    pRef.ReferenceSetter(id);
                    context.PendingReferences.Remove(pRef);
                }
                else
                    full = false;
            return full;
        }
        static public DtoScope GetRelatedDtoScopeFromRootEntity(object root)
        {
            var context = new ConversionContext();
            var dtoScope = getRelatedDtoScope(root, context);
            var rootGuid = context.IdentityMap.GetId(root);
            RootPointer rootPointer = new RootPointer(rootGuid);
            dtoScope.Scope.Add(rootPointer);
            if (!ResolveReferences(context))
                throw new Exception("Some references are not solved");
            return dtoScope;
        }
    }
}
