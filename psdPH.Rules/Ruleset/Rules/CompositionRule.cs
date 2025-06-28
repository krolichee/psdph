using psdPH.Logic.Ruleset.Rules;
using System;
using System.Xml.Serialization;

namespace psdPH.Rules
{
    [Serializable]
    public abstract class CompositionRule : Rule
    {
        
        [XmlIgnore]
        public Composition Composition;
        public delegate void CompositionUpdatedEvent(Composition composition);
        public event CompositionUpdatedEvent CompositionChanged;

        public CompositionRule(Composition composition)
        {
            Composition = composition;
        }
        public virtual void RestoreComposition(Composition composition)
        {
            Composition = composition;
            CompositionChanged?.Invoke(Composition);
        }
        
    }

}
