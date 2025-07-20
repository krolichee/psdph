using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Nodes
{
    public abstract class DtoGuided:Guided
    {
        protected DtoGuided()
        {
            RegistryDto();
        }

        [XmlElement]
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).ApplyDto(this, value);
        }
        protected abstract DtoConverter DtoConverter { get; }
        private void RegistryDto() {
            DtoConvertersRegistry.Register(GetType(),DtoConverter);
        }
    }
}
