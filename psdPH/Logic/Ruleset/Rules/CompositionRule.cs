using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Utils.Setups;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    [Serializable]
    [PsdPhSerializable]
    public abstract class CompositionRule : Rule
    {
        
        [XmlIgnore]
        public Composition Composition;
        public delegate void CompositionUpdatedEvent(Composition composition);
        public event CompositionUpdatedEvent CompositionChanged;

        public virtual event SetupsChangedEvent SetupsChanged;

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
