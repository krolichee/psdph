using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public struct Identity
    {
        public Guid Guid { get; }
        public object Entity { get; }
        public Identity(object entity) : this()
        {
            this.Entity = entity;
            //TODO Точно ли этот класс отсветственен за создание guid?
            Guid = new Guid();
        }

        public Identity(Guid guid, object entity)
        {
            Guid = guid;
            Entity = entity;
        }

        

    }
}
