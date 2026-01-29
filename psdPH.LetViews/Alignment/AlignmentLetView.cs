using psdPH.Lets;
using psdPH.Lets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews
{
    public class AlignmentLetView : LetView
    {
        public AlignmentLetView(Let let):base(let)
        {
            control = new AlignmentLetViewControl(let) { };
            
        }
    }
}
