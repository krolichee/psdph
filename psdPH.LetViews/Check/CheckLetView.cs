using psdPH.Lets;
using psdPH.Lets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews.Check
{
    public class CheckLetView : LetView
    {
        public CheckLetView(Let let) : base(let)
        {
            control = new CheckLetViewControl(let) { };
        }
    }
}
