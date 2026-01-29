using psdPH.Lets;
using psdPH.Lets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews.Choose
{
    public class ChooseLetView : LetView
    {
        public ChooseLetView(Let let, object[] options) : base(let)
        {
            control = new ChooseLetViewControl(let, options);
        }
    }
}
