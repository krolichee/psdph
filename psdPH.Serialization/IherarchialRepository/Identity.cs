using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public struct Identity
    {
        public Identity(Guid guid, object entity)
        {
            Guid = guid;
            Entity = entity;
        }

        public Guid Guid { get; }
        public object Entity { get; }

    }
}
