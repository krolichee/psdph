using psdPH.Logic.Ruleset.Conditions;
using psdPH.Utils.Setups;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    [Serializable]
    public abstract class CompositionCondition :Condition
    {
        [XmlIgnore]
        public Composition Composition;
       

        public void RestoreComposition(Composition composition)
        {
            Composition = composition;
        }
        public CompositionCondition(Composition composition)
        {
            Composition = composition;
            DtoRegistry.Types.Add(this.GetType());
        }
    }
    
}
