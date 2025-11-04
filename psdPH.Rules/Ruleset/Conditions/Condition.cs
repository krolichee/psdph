using psdPH.Logic.Compositions;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Conditions
{
    public abstract class Condition : ISetupable, psdPH.ISerializable
    {
        //Dto
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).GetEntity(this, value);
        }

        //Setups
        public event SetupsChangedEvent SetupsChanged;
        [XmlIgnore]
        public virtual Setup[] Setups => SetupsRegistry.GetFor(this).GetSetups(this);
        public abstract bool IsSetUp();

        //Using
        public abstract bool IsValid();
    }
}
