using Photoshop;
using psdPH.Utils;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    [Serializable]
    [PsdPhSerializable]
    public abstract class Rule : ISetupable, psdPH.ISerializable
    {

        [XmlIgnore]
        public Composition Composition;
        public Rule(Composition composition)
        {
            Composition = composition;
            this.AddTypeToKnownTypes();
        }
        abstract public void Apply(Document doc);

        public virtual void RestoreComposition(Composition composition)
        {
            Composition = composition;
        }

        [XmlIgnore]
        public abstract Setup[] Setups { get; }

        public virtual Rule Clone()
        {
            Rule result = CloneConverter.Clone(this) as Rule;
            result.RestoreComposition(Composition);
            return result;
        }

        public abstract bool IsSetUp();
    }

}
