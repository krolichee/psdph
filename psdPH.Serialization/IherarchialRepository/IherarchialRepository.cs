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
            var context = ScopeConverter.ConvertDtoScope(scope);
            ReferenceResolver.ResolveReferences(context);
            return RootPointer.FindRootPointer(context.IdentityMap).GetRoot(context.IdentityMap);
        }
    }
}
