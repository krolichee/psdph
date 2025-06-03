using System.Xml.Serialization;

namespace psdPH.Logic
{
    public interface ISetupable
    {
        [XmlIgnore]
        Parameter[] Setups { get; }
        bool IsSetUp();
    }
}
