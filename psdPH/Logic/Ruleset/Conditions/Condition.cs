using Photoshop;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    [Serializable]
    [PsdPhSerializable]
    public abstract class Condition : ISetupable,psdPH.ISerializable
    {
        [XmlIgnore]
        public Composition Composition;

        public event SetupsChangedEvent SetupsChanged;

        [XmlIgnore]
        public abstract Setup[] Setups { get; }
        public abstract bool IsValid();

        public void RestoreComposition(Composition composition)
        {
            Composition = composition;
        }
        public virtual bool IsSetUp()=>true;

        public Condition(Composition composition)
        {
            Composition = composition;
            KnownTypes.Types.Add(this.GetType());
        }
    }
    
}
