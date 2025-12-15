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
            var scope = DtoScopeReader.ReadScope(path);
            var rootPointer = RootPointer.FindRootPointer(scope);
            scope.Scope.Remove(rootPointer);
            var context = ScopeConverter.ConvertDtoScope(scope);
            
            ReferenceResolver.ResolveReferences(context);
            return rootPointer.GetRoot(context.IdentityMap);
        }
        public static void WriteRoot(object root, string path)
        {
            var scope = IherarchyConverter.GetRelatedDtoScopeFromRootEntity(root);
            DtoScopeWriter.WriteScope(path,scope);
        }
    }
}
