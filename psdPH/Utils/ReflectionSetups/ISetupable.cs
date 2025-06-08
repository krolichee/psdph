using System.Xml.Serialization;

namespace psdPH.Logic
{
    public interface ISetupable
    {
        [XmlIgnore]
        Setup[] Setups { get; }
        bool IsSetUp();
    }
}
