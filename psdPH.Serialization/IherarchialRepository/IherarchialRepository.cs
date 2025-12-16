using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class IherarchialRepository
    {
        public static object ReadRoot(string path)
        {
            var scope = DtoScopeRepository.ReadScope(path);
            var rootPointer = RootPointer.FindRootPointer(scope);
            scope.Scope.Remove(rootPointer);
            var context = ScopeConverter.ConvertDtoScope(scope);
            
            ReferenceResolver.ResolveReferences(context);
            return rootPointer.GetRoot(context.IdentityMap);
        }
        public static void WriteRoot(string path, object root)
        {
            var scope = IherarchyConverter.GetRelatedDtoScopeFromRootEntity(root);
            DtoScopeRepository.WriteScope(path,scope);
        }
    }
}
