using psdPH.Logic.Compositions;
using psdPH.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public class NullDtoConverter : DtoConverter
    {
        protected override object CreateEntity() => null;

        protected override Dto CreateDto() => null;

        protected override void UpdateDto(object _obj, object _dto){}

        protected override void UpdateEntity(object _obj, object _dto){}
    }
}
