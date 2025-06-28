using psdPH.Logic.Serialization;
using psdPH.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Logic.Compositions
{
    public abstract class DtoConverter
    {
        public abstract object GetDto(object _obj);
        public abstract void ApplyDto(object _obj, object _dto);
        protected virtual void ExportDto(object _obj, object _dto) { }
    }

}

