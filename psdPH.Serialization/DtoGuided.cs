using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    public class DtoGuided:Guided
    {
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).ApplyDto(this, value);
        }
    }
}
