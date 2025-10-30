using psdPH.Logic.Compositions;
using psdPH.Nodes;
using psdPH.Photoshop;
using psdPH.Rules;
using psdPH.Setups;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class Rule:DtoGuided
    {
        //Setups
        public event SetupsChangedEvent SetupsChanged;
        [XmlIgnore]
        public virtual Setup[] Setups => SetupsRegistry.GetFor(this).GetSetups(this);
        public abstract bool IsSetUp();

        //Using
        public abstract void Apply(DocumentWr doc);
        public virtual CompositionRule Clone()
        {
            CompositionRule result = CloneConverter.Clone(this) as CompositionRule;
            return result;
        }
       
    }
}
