using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class Rule:ISerializable,ISetupable
    {
        //Dto
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).ApplyDto(this, value);
        }

        //Setups
        public event SetupsChangedEvent SetupsChanged;
        [XmlIgnore]
        public virtual Setup[] Setups => SetupsRegistry.GetFor(this).GetSetups(this);
        public abstract bool IsSetUp();

        //Using
        abstract public void Apply(Document doc);
        public virtual CompositionRule Clone()
        {
            CompositionRule result = CloneConverter.Clone(this) as CompositionRule;
            return result;
        }
       
    }
}
