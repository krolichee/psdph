using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class NullDtoConverter : DtoConverter
    {
        public override Type DtoType => throw new NotImplementedException();

        public override Type EntityType => throw new NotImplementedException();

        protected override object CreateEntity() => null;

        protected override Dto CreateDto() => null;

        protected override void UpdateDto(object _obj, object _dto){}

        protected override void UpdateEntity(object _obj, object _dto){}
    }
}
