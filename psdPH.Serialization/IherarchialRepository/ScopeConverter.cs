using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class ScopeConverter
    {
        public static ConversionContext ConvertDtoScope(DtoScope dtoScope)
        {
            var identities = new List<Identity>();
            var references = new List<PendingReference>();
            foreach (var item in dtoScope.scope)
            {
                DtoConverter converter = DtoConvertersRegistry.GetForDto(item);
                var identity = converter.GetIdentity(item, out PendingReference[] pReferences);
                identities.Add(identity);
                references.AddRange(pReferences);
            }
            return new ConversionContext(new IdentityMap(identities.ToArray()),references.ToArray());
        }
    }
}
