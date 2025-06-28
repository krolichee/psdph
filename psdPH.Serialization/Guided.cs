using psdPH.Logic.Serialization;
using System;

namespace psdPH.Nodes
{
    public class Guided: ISerializable
    {
       public Guid Guid { get; set; }
        public Guided()
        {
            Guid = Guid.NewGuid();
            GuidScope.Current?.Add(this);
        }
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
