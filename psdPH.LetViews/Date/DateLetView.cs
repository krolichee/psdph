using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews
{
    public class DateLetView : LetView
    {
        public DateLetView(Let let) : base(let)
        {
            control = new DateLetViewControl(let) { };
        }
    }
}
