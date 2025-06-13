using System.Xml.Serialization;

namespace psdPH.Logic
{
    public delegate void SetupsChangedEvent(object sender);
    public interface ISetupable
    {
        [XmlIgnore]
        Setup[] Setups { get; }
        bool IsSetUp();
        event SetupsChangedEvent SetupsChanged;
    }
}
