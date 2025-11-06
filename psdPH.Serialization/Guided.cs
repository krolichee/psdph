using psdPH.Serialization;
using System;

namespace psdPH.Nodes
{
    public class Guided
    {
       public Guid Guid { get; set; }
        public Guided()
        {
            Guid = Guid.NewGuid();
        }
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
