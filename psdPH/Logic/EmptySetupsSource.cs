using psdPH.Logic.Compositions;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public class EmptySetupsSource : SetupsSource
    {
        public override Setup[] GetSetups(object obj) => new Setup[0];
    }
}
