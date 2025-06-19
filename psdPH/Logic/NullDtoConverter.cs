using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public class NullDtoConverter : DtoConverter
    {
        public override void ApplyDto(object _obj, object _dto) { }

        public override object GetDto(object _obj) => null;
    }
}
