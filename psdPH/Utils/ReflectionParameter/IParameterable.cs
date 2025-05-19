using System.Xml.Serialization;

namespace psdPH.Logic
{
    public interface IParameterable
    {
        [XmlIgnore]
        Parameter[] Parameters { get; }
    }
}
